namespace Kigg.Web
{
    using System.Collections.Generic;

    using DomainObjects;

    public class StoryItemViewData
    {
        public IStory Story
        {
            get;
            set;
        }

        public IUser User
        {
            get;
            set;
        }

        public string PromoteText
        {
            get;
            set;
        }

        public string DemoteText
        {
            get;
            set;
        }

        public string CountText
        {
            get;
            set;
        }

        public ICollection<string> SocialServices
        {
            get;
            set;
        }

        public bool DetailMode
        {
            get;
            set;
        }
    }
}