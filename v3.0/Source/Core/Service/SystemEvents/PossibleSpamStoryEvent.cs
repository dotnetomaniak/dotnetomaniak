namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class PossibleSpamStoryEventArgs
    {
        [DebuggerStepThrough]
        public PossibleSpamStoryEventArgs(IStory story, string source, string detailUrl)
        {
            Story = story;
            Source = source;
            DetailUrl = detailUrl;
        }

        public IStory Story
        {
            get;
            private set;
        }

        public string Source
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

    public class PossibleSpamStoryEvent : BaseEvent<PossibleSpamStoryEventArgs>
    {
    }
}