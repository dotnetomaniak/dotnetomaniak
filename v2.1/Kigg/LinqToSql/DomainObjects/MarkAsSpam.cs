namespace Kigg.DomainObjects
{
    using System;

    public partial class StoryMarkAsSpam : IMarkAsSpam
    {
        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

        public IUser ByUser
        {
            get
            {
                return User;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IPAddress;
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