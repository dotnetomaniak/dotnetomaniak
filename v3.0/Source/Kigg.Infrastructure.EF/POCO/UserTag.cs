using System;

namespace Kigg.Infrastructure.EF.POCO
{
    public class UserTag
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}