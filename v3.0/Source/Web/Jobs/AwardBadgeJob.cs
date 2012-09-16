using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Kigg.Infrastructure;
using Kigg.LinqToSql.Repository;
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

            //Shedule awardbadge job to be done in a separate thread.
            //ThreadPool.QueueUserWorkItem(wi =>
                                             {                                                 
                                                 try
                                                 {
                                                     using (var databaseFactory =
                                                         new DatabaseFactory(IoC.Resolve<IConnectionString>()))
                                                     {
                                                         AwardBadges(databaseFactory);
                                                     }
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
            //);
        }

        protected abstract void AwardBadges(IDatabaseFactory databaseFactory);

        protected abstract TimeSpan Interval { get; }

        public void Execute()
        {
        }
    }
}