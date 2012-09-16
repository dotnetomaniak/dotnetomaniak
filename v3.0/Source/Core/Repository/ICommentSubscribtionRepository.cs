namespace Kigg.Repository
{
    using System;

    using DomainObjects;

    public interface ICommentSubscribtionRepository : IRepository<ICommentSubscribtion>, ICountByStoryRepository
    {
        ICommentSubscribtion FindById(Guid storyId, Guid userId);
    }
}