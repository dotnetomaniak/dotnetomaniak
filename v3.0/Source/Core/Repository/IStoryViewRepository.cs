namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IStoryViewRepository : IRepository<IStoryView>, ICountByStoryRepository
    {
        ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp);
    }
}