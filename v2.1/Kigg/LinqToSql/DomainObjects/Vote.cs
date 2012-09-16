namespace Kigg.DomainObjects
{
    using System;

    public partial class StoryVote : IVote
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

        public DateTime PromotedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}