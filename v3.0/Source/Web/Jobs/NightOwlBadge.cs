using System;
using Kigg.Infrastructure.EF;

namespace Kigg.Web.Jobs
{
    public class NightOwlBadge : AwardBadgeJob
    {        
        public Guid Id = new Guid("1FD420BA-3104-436E-A7C8-87897DCFE954");
     
        protected override void AwardBadges(DotnetomaniakContext context)
        {
            //awards a badge if user visits site between 2-3 AM
            context.NightOwl();
        }

        protected override TimeSpan Interval
        {
            get { return new TimeSpan(0, 15, 0); }
        }
    }
}