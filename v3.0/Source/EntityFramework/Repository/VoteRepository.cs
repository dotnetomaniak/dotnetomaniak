namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class VoteRepository : BaseRepository<IVote, StoryVote>, IVoteRepository
    {
        public VoteRepository(IDatabase database)
            : base(database)
        {
        }

        public VoteRepository(IDatabaseFactory factory)
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

            return Database.VoteDataSource.Count(v => v.StoryId == storyId);
        }

#if(DEBUG)
        public virtual IVote FindById(Guid storyId, Guid userId)
#else
        public IVote FindById(Guid storyId, Guid userId)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.VoteDataSource.FirstOrDefault(v => v.StoryId == storyId && v.UserId == userId);
        }

#if(DEBUG)
        public virtual ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp)
#else
        public ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.VoteDataSource
                           .Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                           .AsEnumerable()                          
                           .Cast<IVote>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}