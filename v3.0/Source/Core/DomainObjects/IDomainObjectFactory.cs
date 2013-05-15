using Kigg.Core.DomainObjects;

namespace Kigg.DomainObjects
{
    using System;

    public interface IDomainObjectFactory
    {
        IUser CreateUser(string userName, string email, string password);

        IKnownSource CreateKnownSource(string url);

        ICategory CreateCategory(string name);

        ITag CreateTag(string name);

        IStory CreateStory(ICategory forCategory, IUser byUser, string fromIpAddress, string title, string description, string url);

        IStoryView CreateStoryView(IStory forStory, DateTime at, string fromIpAddress);

        IVote CreateStoryVote(IStory forStory, DateTime at, IUser byUser, string fromIpAddress);

        IMarkAsSpam CreateMarkAsSpam(IStory forStory, DateTime at, IUser byUser, string fromIpAddress);

        IComment CreateComment(IStory forStory, string content, DateTime at, IUser byUser, string fromIpAddress);

        ICommentSubscribtion CreateCommentSubscribtion(IStory forStory, IUser byUser);

        IRecommendation CreateRecommendation(string recommendationLink, string recommendationTitle, string imageLink, string imageTitle, DateTime startTime, DateTime endTime);
    }
}