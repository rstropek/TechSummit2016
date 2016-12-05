using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;
using System.ComponentModel.Composition.Registration;
using Shop.WebApi.DataAccess;
using System.Web.Http;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using Shop.WebApi.Controllers;
using Shop.WebApi.DataAccess.Fakes;
using Shop.WebApi.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.WebApi;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace Shop.Tests
{
    [TestClass]
    public class ProductApiTest
    {
        public class StartupMockup
        {
            public void Configuration(IAppBuilder app)
            {
                // Setup dependency injection
                var registrationBuilder = new RegistrationBuilder();
                registrationBuilder.ForTypesDerivedFrom<ApiController>().Export().SetCreationPolicy(CreationPolicy.NonShared);
                var catalog = new AssemblyCatalog(typeof(ProductsController).Assembly, registrationBuilder);
                var container = new CompositionContainer(catalog, true);
                container.ComposeExportedValue<IProductTable>(new StubIProductTable
                {
                    GetAllProductsAsync = () => Task.FromResult<IEnumerable<Product>>(new[]
                    {
                        new Product { ProductId = 1, Description = "Dummy", UnitPrice = 10M }
                    })
                });

                // Enable Web API
                var configuration = new HttpConfiguration();
                configuration.DependencyResolver = new MefDependencyResolver(container);
                configuration.MapHttpAttributeRoutes();
                app.UseWebApi(configuration);
            }
        }

        [TestMethod]
        public async Task TestProductApi()
        {
            using (WebApp.Start<StartupMockup>("http://localhost:9000/"))
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://localhost:9000/api/products");
                var products = await response.Content.ReadAsAsync<Product[]>();
                Assert.AreEqual(1, products.Length);
                Assert.AreEqual("Dummy", products[0].Description);
            }
        }

        [TestMethod]
        public void TestProductEntityToModelConversion()
        {
            var pe = new ProductEntity
            {
                RowKey = "1",
                Description = "Dummy",
                UnitPrice = 100
            };
            var p = pe.ToModel();
            Assert.AreEqual(1.0M, p.UnitPrice);
        }

        [Ignore]
        [TestMethod]
        public void FailingTest()
        {
            Assert.Fail();
        }
    }
}
