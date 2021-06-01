using System;
using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class CommentSubscribtion: ICommentSubscribtion
    {
        public Guid StoryId { get; set; }
        public virtual Story Story { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        public IStory ForStory => Story;

        [NotMapped]
        public IUser ByUser => User;
    }
}