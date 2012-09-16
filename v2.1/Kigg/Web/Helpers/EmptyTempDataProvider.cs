namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class EmptyTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            return new Dictionary<string, object>();
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
        }
    }
}