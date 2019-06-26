using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class FacebookBadge : AwardBadgeJob
    {        
        public Guid Id = new Guid("856E4164-AB34-4221-8E23-100B0C6BE576");        

        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.Facebook();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 12, 0); }
        }
    }
}