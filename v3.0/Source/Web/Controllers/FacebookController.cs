using Kigg.Web.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Kigg.Repository;
using Kigg.Infrastructure;

namespace Kigg.Web.Controllers
{
    public class FacebookController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public FacebookController(IUserRepository userRepository)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");

            _userRepository=userRepository;
        }

        //[AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult GetFBData(string data)
        {
            var myself = new JavaScriptSerializer().Deserialize<FbUserDataView>(data);            

            using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {                
                var user = _userRepository.FindByFbId(myself.Id);

                if (user != null)
                {
                    user.LastActivityAt = SystemTime.Now();
                    unitOfWork.Commit();
                   
                    FormsAuthentication.SetAuthCookie(user.UserName, false);                    

                }

            }
            return Json(new {status = "hura" });
        }
    }
}
