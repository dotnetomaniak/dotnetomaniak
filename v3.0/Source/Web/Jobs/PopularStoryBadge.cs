using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class PopularStoryBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("0412DD1F-0F6A-417E-B72F-36F2E5D9DDE4");
        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.PopularStory();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 9, 0); }
        }
    }
}