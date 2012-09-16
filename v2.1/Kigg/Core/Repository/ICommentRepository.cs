namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICommentRepository : IRepository<IComment>
    {
        int CountByStory(Guid storyId);

        IComment FindById(Guid storyId, Guid commentId);

        ICollection<IComment> FindAfter(Guid storyId, DateTime timestamp);
    }
}