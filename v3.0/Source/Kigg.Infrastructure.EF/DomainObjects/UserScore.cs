﻿namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class UserScore
    {
        public long Id { get; set; }
        public System.Guid UserId { get; set; }

        public System.DateTime Timestamp { get; set; }

        public Kigg.DomainObjects.UserAction ActionType { get; set; }

        public decimal Score { get; set; }

        public virtual User User { get; set; }
    }
}