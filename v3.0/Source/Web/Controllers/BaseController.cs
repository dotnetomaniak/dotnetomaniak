namespace Kigg.Web
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public abstract class BaseController : Controller
    {
        private static readonly Type CurrentUserKey = typeof(IUser);

        protected BaseController()
        {
            TempDataProvider = new EmptyTempDataProvider();
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public IFormsAuthentication FormsAuthentication
        {
            get;
            set;
        }

        public IUserRepository UserRepository
        {
            get;
            set;
        }

        public string CurrentUserName
        {
            [DebuggerStepThrough]
            get
            {
                return (HttpContext.User == null) ? null : HttpContext.User.Identity.Name;
            }
        }

        public IUser CurrentUser
        {
            get
            {
                if (!string.IsNullOrEmpty(CurrentUserName))
                {
                    IUser user = HttpContext.Items[CurrentUserKey] as IUser;

                    if (user == null)
                    {
                        using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                        {
                            user = UserRepository.FindByUserName(CurrentUserName);

                            if (user != null)
                            {
                                try
                                {
                                    if (!user.IsLockedOut)
                                    {
                                        user.LastActivityAt = SystemTime.Now();
                                        unitOfWork.Commit();
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Exception(e);
                                }

                                HttpContext.Items[CurrentUserKey] = user;
                            }
                        }
                    }
                    return user;
                }

                return null;
            }
        }

        public bool IsCurrentUserAuthenticated
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated && (CurrentUser != null))
                {
                    if (!CurrentUser.IsLockedOut)
                    {
                        return true;
                    }

                    Log.Warning("Logging out User: {0}", CurrentUserName);

                    // Logout the user if the account is locked out
                    FormsAuthentication.SignOut();

                    Log.Info("User Logged out.");
                }

                return false;
            }
        }

        public string CurrentUserIPAddress
        {
            get
            {
                return HttpContext.Request.UserHostAddress;
            }
        }

        public T CreateViewData<T>() where T : BaseViewData, new()
        {
            T viewData = new T
                             {
                                 SiteTitle = Settings.SiteTitle,
                                 RootUrl = Settings.RootUrl,
                                 MetaKeywords = Settings.MetaKeywords,
                                 MetaDescription = Settings.MetaDescription,
                                 IsCurrentUserAuthenticated = IsCurrentUserAuthenticated,
                                 CurrentUser = CurrentUser
                             };

            return viewData;
        }

        public static T Validate<T>(params Validation[] validations) where T : JsonViewData, new()
        {
            foreach (Validation validation in validations)
            {
                bool result = validation.Expression.Compile().Invoke();

                if (result)
                {
                    return new T { errorMessage = validation.ErrorMessage };
                }
            }

            return null;
        }

        public static void ThrowNotFound(string message)
        {
            throw new HttpException((int) HttpStatusCode.NotFound, message);
        }
    }
}