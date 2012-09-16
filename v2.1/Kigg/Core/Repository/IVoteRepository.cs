namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IVoteRepository : IRepository<IVote>
    {
        int CountByStory(Guid storyId);

        IVote FindById(Guid storyId, Guid userId);

        ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp);
    }
}