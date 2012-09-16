namespace Kigg.Web
{
    public interface IFormsAuthentication
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);

        void SignOut();
    }
}