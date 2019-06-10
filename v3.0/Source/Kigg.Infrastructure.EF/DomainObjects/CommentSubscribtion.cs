using System;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class CommentSubscribtion: Entity, ICommentSubscribtion
    {
        public Guid StoryId { get; set; }
        public Story Story { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public IStory ForStory => Story;

        [NotMapped]
        public IUser ByUser => User;
    }
}