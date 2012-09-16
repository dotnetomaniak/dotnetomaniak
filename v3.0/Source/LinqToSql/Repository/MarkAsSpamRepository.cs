namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class MarkAsSpamRepository : BaseRepository<IMarkAsSpam, StoryMarkAsSpam>, IMarkAsSpamRepository
    {
        public MarkAsSpamRepository(IDatabase database) : base(database)
        {
        }

        public MarkAsSpamRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.MarkAsSpamDataSource.Count(m => m.StoryId == storyId);
        }

        public virtual IMarkAsSpam FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.MarkAsSpamDataSource.SingleOrDefault(m => m.StoryId == storyId && m.UserId == userId);
        }

        public virtual ICollection<IMarkAsSpam> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.MarkAsSpamDataSource.Where(m => m.StoryId == storyId && m.Timestamp >= timestamp)
                                                .Cast<IMarkAsSpam>()
                                                .ToList()
                                                .AsReadOnly();
        }
    }
}