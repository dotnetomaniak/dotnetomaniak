using System;

namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryMarkAsSpam : IMarkAsSpam
    {
        [NonSerialized]
        private EntityReference<IStory, Story> _storyReference;
        [NonSerialized]
        private EntityReference<IUser, User> _userReference;
        public IStory ForStory
        {
            get
            {
                if(Story == null)
                {
                    EntityHelper.EnsureEntityReference(ref _storyReference, StoryReference);
                    EntityHelper.EnsureEntityReferenceLoaded(_storyReference);
                }
                return Story;
            }
        }

        public IUser ByUser
        {
            get
            {
                if (User == null)
                {
                    EntityHelper.EnsureEntityReference(ref _userReference, UserReference);
                    EntityHelper.EnsureEntityReferenceLoaded(_userReference);
                }
                return User;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IpAddress;
            }
        }

        public DateTime MarkedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}
