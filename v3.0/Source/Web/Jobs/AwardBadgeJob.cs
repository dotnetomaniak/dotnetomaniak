using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Kigg.Infrastructure;
using Kigg.Infrastructure.EF;
using Cache = System.Web.Caching.Cache;

namespace Kigg.Web.Jobs
{
    public abstract class AwardBadgeJob : IBootstrapperTask
    {
        protected AwardBadgeJob()
        {
            Insert();
        }

        private void Insert()
        {
            HttpRuntime.Cache.Add(GetType().ToString(),
                                  this,
                                  null, Cache.NoAbsoluteExpiration,
                                  Interval,
                                  CacheItemPriority.Normal,
                                  Callback);
        }

        private void Callback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason != CacheItemRemovedReason.Expired)
                return;

            try
            {
                var context = IoC.Resolve<DotnetomaniakContext>();
                AwardBadges(context);
            }
            catch (Exception e)
            {
                var logger = IoC.Resolve<ILog>();
                logger.Exception(e);
            }
            finally
            {
                Insert();
            }
        }

        protected abstract void AwardBadges(DotnetomaniakContext context);

        protected abstract TimeSpan Interval { get; }

        public void Execute()
        {
        }
    }
}