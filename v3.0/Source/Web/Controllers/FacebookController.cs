using Kigg.Web.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Kigg.Web.Controllers
{
    public class FacebookController : BaseController
    {
        //[AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult GetFBData(string data)
        {
            var myself = new JavaScriptSerializer().Deserialize<FbUserDataView>(data);                
            
            return Json(new {status = "hura" });
        }
    }
}
