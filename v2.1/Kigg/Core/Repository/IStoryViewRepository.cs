namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IStoryViewRepository : IRepository<IStoryView>
    {
        int CountByStory(Guid storyId);

        ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp);
    }
}