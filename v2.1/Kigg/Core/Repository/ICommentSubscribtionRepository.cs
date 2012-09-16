namespace Kigg.Repository
{
    using System;

    using DomainObjects;

    public interface ICommentSubscribtionRepository : IRepository<ICommentSubscribtion>
    {
        int CountByStory(Guid storyId);

        ICommentSubscribtion FindById(Guid storyId, Guid userId);
    }
}