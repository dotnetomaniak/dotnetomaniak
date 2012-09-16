namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class StorySubmitEventArgs
    {
        [DebuggerStepThrough]
        public StorySubmitEventArgs(IStory story, string detailUrl)
        {
            Story = story;
            DetailUrl = detailUrl;
        }

        public IStory Story
        {
            get;
            private set;
        }

        public string DetailUrl
        {
            get;
            private set;
        }
    }

    public class StorySubmitEvent : BaseEvent<StorySubmitEventArgs>
    {
    }
}