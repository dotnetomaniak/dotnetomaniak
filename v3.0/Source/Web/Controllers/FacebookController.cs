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
        public ActionResult LogByFbData(string data)
        {
            JsonViewData viewData = Validate<JsonViewData>(
                new Validation(() => string.IsNullOrEmpty(data.NullSafe()), "Nie udało nam się uzyskać Twoich danych z serwisu Facebook, spróbuj ponownie.")
                                                          );
            if (viewData == null)
            {
                try
                {                    
                    var fbUserViewData = new JavaScriptSerializer().Deserialize<FbUserDataView>(data);
                    fbUserViewData = AssignViewData<FbUserDataView>(fbUserViewData);

                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        var user = _userRepository.FindByFbId(fbUserViewData.Id);

                        if (user != null)
                        {
                            viewData = LogUserByFb(viewData, unitOfWork, user);
                        }
                        else
                        {                            
                            user = _userRepository.FindByEmail(fbUserViewData.Email);

                            if (user != null)
                            {
                                viewData = LogUserByFb(viewData, unitOfWork, user);

                                user.FbId = fbUserViewData.Id;
                                unitOfWork.Commit();
                            }
                            else
                            {

                                return Json(new { redirectUrl = Url.Action("FbLog", "Facebook"), isSuccessful = true, isRedirect = true });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("logowania") };
                }         
            }
            return Json(viewData);
        }


        private JsonViewData LogUserByFb(JsonViewData viewData, IUnitOfWork unitOfWork, DomainObjects.IUser user)
        {
            viewData = Validate<JsonViewData>(
                                                new Validation(() => user.IsLockedOut, "Twoje konto jest aktualnie zablokowane. Skontaktuj się z pomocą aby rozwiązać ten problem."),
                                                new Validation(() => !user.IsActive, "Twoje konto nie zostało jeszcze aktywowane. Posłóż się linkiem aktywacyjnym z wiadomości rejestracyjnej aby aktywować konto.")
                                             );

            if (user != null)
            {
                user.LastActivityAt = SystemTime.Now();
                unitOfWork.Commit();

                FormsAuthentication.SetAuthCookie(user.UserName, false);
                viewData = new JsonViewData { isSuccessful = true };

                Log.Info("Użytkownik zalogowany: {0}", user.UserName);
            }
            return viewData;
        }

        [Compress]
        public ActionResult FbLog()
        {
            return View();
        }
    }
}
