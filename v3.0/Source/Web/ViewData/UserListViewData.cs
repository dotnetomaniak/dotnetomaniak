namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using DomainObjects;

    public class UserListViewData : BaseViewData
    {
        public UserListViewData()
        {
            Users = new ReadOnlyCollection<IUser>(new List<IUser>());
        }

        public int PageCount
        {
            [DebuggerStepThrough]
            get
            {
                return PageCalculator.TotalPage(TotalUserCount, UserPerPage);
            }
        }

        public string Title
        {
            get;
            set;
        }

        public string Subtitle
        {
            get;
            set;
        }

        public string NoUserExistMessage
        {
            get;
            set;
        }

        public int UserPerPage
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public ICollection<IUser> Users
        {
            get;
            set;
        }

        public int TotalUserCount
        {
            get;
            set;
        }
    }
}