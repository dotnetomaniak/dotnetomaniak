namespace Kigg.LinqToSql.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;
    
    public class VoteRepository : BaseRepository<IVote, StoryVote>, IVoteRepository
    {
        public VoteRepository(IDatabase database) : base(database)
        {
        }

        public VoteRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.VoteDataSource.Count(v => v.StoryId == storyId);
        }

        public virtual IVote FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.VoteDataSource.SingleOrDefault(v => v.StoryId == storyId && v.UserId == userId);
        }

        public virtual ICollection<IVote> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.VoteDataSource.Where(v => v.StoryId == storyId && v.Timestamp >= timestamp)
                                          .Cast<IVote>()
                                          .ToList()
                                          .AsReadOnly();
        }
    }
}