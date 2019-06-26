﻿using System;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class UserTag: Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}