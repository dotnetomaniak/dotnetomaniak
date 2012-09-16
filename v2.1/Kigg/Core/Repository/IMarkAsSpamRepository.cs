namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IMarkAsSpamRepository : IRepository<IMarkAsSpam>
    {
        int CountByStory(Guid storyId);

        IMarkAsSpam FindById(Guid storyId, Guid userId);

        ICollection<IMarkAsSpam> FindAfter(Guid storyId, DateTime timestamp);
    }
}