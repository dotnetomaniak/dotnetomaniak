using System;

namespace Kigg.Web
{
    using DomainObjects;

    public class StoryListUserViewData : StoryListViewData
    {
        public UserDetailTab SelectedTab
        {
            get;
            set;
        }

        public IUser TheUser
        {
            get;
            set;
        }
    }
}