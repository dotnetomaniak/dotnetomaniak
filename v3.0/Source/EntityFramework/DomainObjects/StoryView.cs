using System;

namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryView : IStoryView
    {
        [NonSerialized]
        private EntityReference<IStory, Story> _storyReference;
        public IStory ForStory
        {
            get
            {
                if (Story == null)
                {
                    EntityHelper.EnsureEntityReference(ref _storyReference, StoryReference);
                    EntityHelper.EnsureEntityReferenceLoaded(_storyReference);
                }
                return Story;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IpAddress;
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
