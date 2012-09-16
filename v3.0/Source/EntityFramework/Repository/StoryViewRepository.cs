namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class StoryViewRepository : BaseRepository<IStoryView, StoryView>, IStoryViewRepository
    {
        public StoryViewRepository(IDatabase database)
            : base(database)
        {
        }

        public StoryViewRepository(IDatabaseFactory factory)
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

            return Database.StoryViewDataSource.Count(v => v.Story.Id == storyId);
        }
#if(DEBUG)
        public virtual ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp)
#else
        public ICollection<IStoryView> FindAfter(Guid storyId, DateTime timestamp)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.StoryViewDataSource
                           .Where(v => v.Story.Id == storyId && v.Timestamp >= timestamp)
                           .AsEnumerable()                               
                           .Cast<IStoryView>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}