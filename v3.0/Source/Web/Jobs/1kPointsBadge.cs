using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class _1kPointsBadge : AwardBadgeJob
    {        
        public Guid Id = new Guid("77287DE2-5F75-483F-BE1F-A544D266CFD8");        

        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context._1kPoints();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 4, 0); }
        }
    }
}