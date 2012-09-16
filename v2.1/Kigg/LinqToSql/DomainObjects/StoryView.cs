namespace Kigg.DomainObjects
{
    using System;

    public partial class StoryView : IStoryView
    {
        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IPAddress;
            }
        }

        public DateTime ViewedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}