namespace Kigg.Web
{
    using DomainObjects;

    public class UserWithScore
    {
        public IUser User
        {
            get;
            set;
        }

        public decimal Score
        {
            get;
            set;
        }
    }
}