namespace Kigg.Service
{
    using System.Diagnostics;
    using DomainObjects;
    using Infrastructure;

    public class StoryUnmarkAsSpamEventArgs
    {
        [DebuggerStepThrough]
        public StoryUnmarkAsSpamEventArgs(IStory story, IUser user)
        {
            Story = story;
            User = user;
        }

        public IStory Story
        {
            get;
            private set;
        }

        public IUser User
        {
            get;
            private set;
        }
    }

    public class StoryUnmarkAsSpamEvent : BaseEvent<StoryUnmarkAsSpamEventArgs>
    {
    }
}