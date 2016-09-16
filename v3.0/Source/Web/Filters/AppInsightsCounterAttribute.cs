using System.Web.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Kigg.Web
{
    public class AppInsightsCounterAttribute : ActionFilterAttribute
    {
        private string counterName;
        private static TelemetryClient tc = new TelemetryClient();

        public AppInsightsCounterAttribute(string counterName)
        {
            this.counterName = counterName;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            tc.TrackMetric(new MetricTelemetry(counterName, 1));
            base.OnActionExecuting(filterContext);
        }
    }
}