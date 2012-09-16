namespace Kigg.Infrastructure
{
    using DomainObjects;

    public class DefaultUser
    {
        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public Roles Role
        {
            get;
            set;
        }
    }
}