using System;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class UserTag
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}