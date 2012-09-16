namespace Kigg.Service
{
    using DomainObjects;

    public class StoryCreateResult : BaseServiceResult
    {
        public IStory NewStory
        {
            get;
            set;
        }

        public string DetailUrl
        {
            get;
            set;
        }
    }
}