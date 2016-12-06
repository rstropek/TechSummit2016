using Microsoft.Owin;
using Owin;
using System.Web.Http;
using ApplicationInsights.OwinExtensions;
using Swashbuckle.Application;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shop.WebApi.AzureUtilities;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Shop.WebApi.DataAccess;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(Shop.WebApi.Startup))]

namespace Shop.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Initialize data store
            new DataStoreInitializer().InitializeDataStoreAsync().Wait();

            // Add CORS to be able to call Web API from other hosts
            app.UseCors(CorsOptions.AllowAll);

            // Enable application insights
            app.UseApplicationInsights();

            // Configure JSON formatter
            var settings = GlobalConfiguration.Configuration.Formatters
                .JsonFormatter
                .SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Setup dependency injection
            var registrationBuilder = new RegistrationBuilder();
            registrationBuilder.ForType<ProductTable>().Export<IProductTable>().SetCreationPolicy(CreationPolicy.NonShared);
            registrationBuilder.ForType<OrderTable>().Export<IOrderTable>().SetCreationPolicy(CreationPolicy.NonShared);
            registrationBuilder.ForTypesDerivedFrom<ApiController>().Export().SetCreationPolicy(CreationPolicy.NonShared);
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly(), registrationBuilder);
            var container = new CompositionContainer(catalog, true);

            // Enable Web API
            var configuration = new HttpConfiguration();
            configuration.DependencyResolver = new MefDependencyResolver(container);
            configuration.MapHttpAttributeRoutes();
            configuration.Services.Add(typeof(IExceptionLogger), new ApplicationInsightsExceptionLogger(new TelemetryClient()));
            configuration
                .EnableSwagger(config => config.SingleApiVersion("v1", "Technical Summit Shop API"))
                .EnableSwaggerUi();
            app.UseWebApi(configuration);
        }
    }
}
