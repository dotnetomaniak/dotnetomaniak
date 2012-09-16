namespace Kigg.DomainObjects
{
    public partial class StoryComment : IComment
    {
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

        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

        public virtual void MarkAsOffended()
        {
            IsOffended = true;
        }
    }
}