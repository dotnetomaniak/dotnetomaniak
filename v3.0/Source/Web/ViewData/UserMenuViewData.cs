namespace Kigg.Web
{
    using DomainObjects;

    public class UserMenuViewData
    {
        public bool IsUserAuthenticated
        {
            get;
            set;
        }

        public IUser CurrentUser
        {
            get;
            set;
        }
    }
}