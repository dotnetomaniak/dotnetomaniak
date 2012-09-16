namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using DomainObjects;

    public class FeedViewData
    {
        public FeedViewData()
        {
            Stories = new ReadOnlyCollection<IStory>(new List<IStory>());
        }

        public string SiteTitle
        {
            get;
            set;
        }

        public string RootUrl
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string PromoteText
        {
            get;
            set;
        }

        public int CacheDurationInMinutes
        {
            get;
            set;
        }

        public ICollection<IStory> Stories
        {
            get;
            set;
        }

        public int Start
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public int TotalStoryCount
        {
            get;
            set;
        }
    }
}