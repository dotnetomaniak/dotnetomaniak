namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Infrastructure;

    public class StoryPublishEventArgs
    {
        [DebuggerStepThrough]
        public StoryPublishEventArgs(ICollection<PublishedStory> publishedStories, DateTime timestamp)
        {
            PublishedStories = publishedStories;
            Timestamp = timestamp;
        }

        public ICollection<PublishedStory> PublishedStories
        {
            get;
            private set;
        }

        public DateTime Timestamp
        {
            get;
            private set;
        }
    }

    public class StoryPublishEvent : BaseEvent<StoryPublishEventArgs>
    {
    }
}