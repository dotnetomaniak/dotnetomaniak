using System;
using System.Collections.Generic;
using System.Linq;
using Kigg.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class CommentRepository: BaseRepository<StoryComment>, ICommentRepository
    {
        private readonly DotnetomaniakContext _context;

        public CommentRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(IComment entity)
        {
            base.Add((StoryComment)entity);
        }

        public void Remove(IComment entity)
        {
            base.Remove((StoryComment)entity);
        }

        public int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return _context.StoryComments.Count(c => c.StoryId == storyId);
        }

        public IComment FindById(Guid storyId, Guid commentId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(commentId, "commentId");

            return _context.StoryComments.SingleOrDefault(c => c.Id == commentId && c.StoryId == storyId);
        }

        public ICollection<IComment> FindAfter(Guid storyId, DateTime timestamp)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            return _context.StoryComments.Where(c => c.StoryId == storyId && c.CreatedAt >= timestamp)
                                             .Cast<IComment>()
                                             .ToList()
                                             .AsReadOnly();
        }
    }
}