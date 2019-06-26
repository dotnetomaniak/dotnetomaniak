using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class StoryAdderBadge : AwardBadgeJob
    {
        readonly Guid Id = new Guid("2E8F0603-0EB3-4412-A864-854448B894BE");
        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.StoryAdder();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 3, 0); }
        }
    }
}