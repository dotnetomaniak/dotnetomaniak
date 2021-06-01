using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class CommenterBadge : AwardBadgeJob
    {
        public Guid Id = new Guid("F23FF0BA-37F1-433B-B405-7427D0F4ED2E");
        protected override void AwardBadges(DotnetomaniakContext context)
        {
            context.Commenter();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 10, 0); }
        }
    }
}