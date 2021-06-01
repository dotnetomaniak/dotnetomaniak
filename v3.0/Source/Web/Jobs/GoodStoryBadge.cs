using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class GoodStoryBadge : AwardBadgeJob
    {
        public Guid id = new Guid("134B2022-274F-46AB-98F5-2BE0CBC972AC");
        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.GoodStory();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 13, 0); }
        }
    }
}