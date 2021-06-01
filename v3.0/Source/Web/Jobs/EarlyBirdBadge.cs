﻿using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class EarlyBirdBadge : AwardBadgeJob
    {        
        public readonly Guid Id = new Guid("F5CC5705-5559-4818-A440-8C77F7D0B30C");        

        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.EarlyBird();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 12, 0); }

        }
    }
}