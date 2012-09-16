namespace Kigg.Web
{
    using System.Collections.Generic;

    using DomainObjects;

    public class UserDetailViewData : BaseViewData
    {
        public IUser TheUser
        {
            get;
            set;
        }

        public IDictionary<string, bool> IPAddresses
        {
            get;
            set;
        }

        public UserDetailTab SelectedTab
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }
    }
}