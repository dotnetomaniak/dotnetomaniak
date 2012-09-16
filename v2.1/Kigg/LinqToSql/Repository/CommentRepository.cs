namespace Kigg.Repository.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;

    public class CommentRepository : BaseRepository<IComment, StoryComment>, ICommentRepository
    {
        public CommentRepository(IDatabase database) : base(database)
        {
        }

        public CommentRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public virtual int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.CommentDataSource.Count(c => c.StoryId == storyId);
        }

        public virtual IComment FindById(Guid storyId, Guid commentId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(commentId, "commentId");

            //
            return Database.CommentDataSource.SingleOrDefault(c => c.Id == commentId && c.StoryId == storyId);
        }

        public virtual ICollection<IComment> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return Database.CommentDataSource.Where(c => c.StoryId == storyId && c.CreatedAt >= timestamp)
                                             .Cast<IComment>()
                                             .ToList()
                                             .AsReadOnly();
        }
    }
}