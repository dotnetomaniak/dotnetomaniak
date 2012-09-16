namespace Kigg.DomainObjects
{
    public partial class CommentSubscribtion : ICommentSubscribtion
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
    }
}