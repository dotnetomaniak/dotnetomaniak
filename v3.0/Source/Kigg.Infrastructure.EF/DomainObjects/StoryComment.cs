using System.ComponentModel.DataAnnotations.Schema;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class StoryComment: Entity, IComment
    {
        public string HtmlBody { get; set; }

        public virtual string TextBody { get; set; }

        public System.Guid StoryId { get; set; }
        public virtual Story Story { get; set; }

        public System.Guid UserId { get; set; }
        public virtual User User { get; set; }

        public string IPAddress { get; set; }

        public bool IsOffended { get; set; }

        [NotMapped]
        public IStory ForStory => Story;

        [NotMapped]
        public IUser ByUser => User;

        [NotMapped]
        public string FromIPAddress => IPAddress;

        public virtual void MarkAsOffended() => IsOffended = true;
    }
}