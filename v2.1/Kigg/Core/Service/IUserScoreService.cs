namespace Kigg.Service
{
    using DomainObjects;

    public interface IUserScoreService
    {
        void AccountActivated(IUser ofUser);

        void StorySubmitted(IUser byUser);

        void StoryViewed(IStory theStory, IUser byUser);

        void StoryPromoted(IStory theStory, IUser byUser);

        void StoryDemoted(IStory theStory, IUser byUser);

        void StoryMarkedAsSpam(IStory theStory, IUser byUser);

        void StoryUnmarkedAsSpam(IStory theStory, IUser byUser);

        void StoryCommented(IStory theStory, IUser byUser);

        void StoryDeleted(IStory theStory);

        void StoryPublished(IStory theStory);

        void StorySpammed(IStory ofUser);

        void StoryIncorrectlyMarkedAsSpam(IUser byUser);

        void CommentSpammed(IUser ofUser);

        void CommentMarkedAsOffended(IUser ofUser);
    }
}