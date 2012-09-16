namespace Kigg.EF.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using DomainObjects;

    public class CommentRepository : BaseRepository<IComment, StoryComment>, ICommentRepository
    {
        public CommentRepository(IDatabase database)
            : base(database)
        {
        }

        public CommentRepository(IDatabaseFactory factory)
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

            return Database.CommentDataSource.Count(c => c.Story.Id == storyId);
        }

#if(DEBUG)
        public virtual IComment FindById(Guid storyId, Guid commentId)
#else
        public IComment FindById(Guid storyId, Guid commentId)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(commentId, "commentId");

            return Database.CommentDataSource.FirstOrDefault(c => c.Id == commentId && c.Story.Id == storyId);
        }

#if(DEBUG)
        public virtual ICollection<IComment> FindAfter(Guid storyId, DateTime timestamp)
#else
        public ICollection<IComment> FindAfter(Guid storyId, DateTime timestamp)
#endif
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.CommentDataSource
                           .Where(c => c.Story.Id == storyId && c.CreatedAt >= timestamp)
                           .AsEnumerable()
                           .Cast<IComment>()
                           .ToList()
                           .AsReadOnly();
        }
    }
}