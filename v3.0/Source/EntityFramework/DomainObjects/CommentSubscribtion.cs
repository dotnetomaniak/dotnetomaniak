namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public class CommentSubscribtion : ICommentSubscribtion
    {
        private readonly Story _story;
        private readonly User _user;
        public CommentSubscribtion (Story forStory, User byUser)
        {
            _story = forStory;
            _user = byUser;
        }
        
        public IStory ForStory
        {
            get
            {
                return _story;
            }
        }

        public IUser ByUser
        {
            get
            {
                return _user;
            }
        }
    }
}