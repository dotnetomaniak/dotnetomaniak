namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class StoryViewRepository : BaseRepository<IStoryView, StoryView>, IStoryViewRepository
    {
        public StoryViewRepository(IDatabase database) : base(database)
        {
        }

        public StoryViewRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.StoryViewDataSource.Count(v => v.StoryId == storyId);
        }

        public virtual ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.StoryViewDataSource.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                               .Cast<IStoryView>()
                                               .ToList()
                                                .AsReadOnly();
        }
    }
}