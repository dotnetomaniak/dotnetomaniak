namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class MarkAsSpamRepository : BaseRepository<IMarkAsSpam, StoryMarkAsSpam>, IMarkAsSpamRepository
    {
        public MarkAsSpamRepository(IDatabase database)
            : base(database)
        {
        }

        public MarkAsSpamRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

#if(DEBUG)
        public virtual int CountByStory(Guid storyId)
#else
        public int CountByStory(Guid storyId)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.MarkAsSpamDataSource.Count(m => m.StoryId == storyId);
        }

#if(DEBUG)
        public virtual IMarkAsSpam FindById(Guid storyId, Guid userId)
#else
        public IMarkAsSpam FindById(Guid storyId, Guid userId)
#endif

        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.MarkAsSpamDataSource.FirstOrDefault(m => m.StoryId == storyId && m.UserId == userId);
        }

#if(DEBUG)
        public virtual ICollection<IMarkAsSpam> FindAfter(Guid storyId, DateTime timestamp)
#else
        public ICollection<IMarkAsSpam> FindAfter(Guid storyId, DateTime timestamp)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.MarkAsSpamDataSource.Where(m => m.StoryId == storyId && m.Timestamp >= timestamp)
                                                .AsEnumerable()
                                                .Cast<IMarkAsSpam>()
                                                .ToList()
                                                .AsReadOnly();
        }
    }
}