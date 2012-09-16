namespace Kigg.DomainObjects
{
    public interface ICommentSubscribtion
    {
        IStory ForStory
        {
            get;
        }

        IUser ByUser
        {
            get;
        }
    }
}