using System.Web.Mvc;

namespace Kigg.Web.Test
{
    public class ControllerTestDouble : Controller
    {
        public ActionResult DummyMethod()
        {
            return View();
        }
    }
}