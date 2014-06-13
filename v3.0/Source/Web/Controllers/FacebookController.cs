using Kigg.Web.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Kigg.Repository;
using Kigg.Infrastructure;
using Kigg.DomainObjects;
using Kigg.Service;

namespace Kigg.Web.Controllers
{
    public class FacebookController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IDomainObjectFactory _factory;
        private readonly IEventAggregator _eventAggregator;

        public FacebookController(IUserRepository userRepository, IDomainObjectFactory factory, IEventAggregator eventAggregator)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");

            _userRepository = userRepository;
            _factory = factory;
            _eventAggregator = eventAggregator;
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

                                return Json(new { redirectUrl = Url.Action("FbLog", "Facebook",null, "http"), isSuccessful = true, isRedirect = true });
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

        public ActionResult FbLog(string id)
        {
            var fbIdViewData = new FbIdViewData();
            fbIdViewData.Id = id;
            fbIdViewData = AssignViewData<FbIdViewData>(fbIdViewData);

            return View(fbIdViewData);
        }

        public ActionResult CreateUserByFb(string data)
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
                        IUser user = _factory.CreateUser(fbUserViewData.Name, fbUserViewData.Email, null);
                        user.FbId = fbUserViewData.Id;
                        user.LastActivityAt = SystemTime.Now();
                        UserRepository.Add(user);

                        _eventAggregator.GetEvent<UserActivateEvent>().Publish(new UserActivateEventArgs(user));
                        
                        unitOfWork.Commit();
                        viewData = LogUserByFb(viewData, unitOfWork, user);
                        
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("logowania") };
                }                


            }

            return Json("abcd");
        }
    }
}
