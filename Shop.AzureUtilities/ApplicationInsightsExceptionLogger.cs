using Microsoft.ApplicationInsights;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Shop.WebApi.AzureUtilities
{
    public class ApplicationInsightsExceptionLogger : ExceptionLogger
    {
        readonly TelemetryClient telemetryClient;

        public ApplicationInsightsExceptionLogger(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public override Task LogAsync(ExceptionLoggerContext context, System.Threading.CancellationToken cancellationToken)
        {
            telemetryClient.TrackException(context.Exception);
            return Task.FromResult<object>(null);
        }

        public override void Log(ExceptionLoggerContext context)
        {
            telemetryClient.TrackException(context.Exception);
        }
    }
}