using System;
using System.Linq;
using Kigg.Core.DomainObjects;
using Kigg.Infrastructure;
using Kigg.LinqToSql.DomainObjects;
using Kigg.Repository;

namespace Kigg.LinqToSql.Repository
{
    public class CommingEventRepository : BaseRepository<ICommingEvent, CommingEvent>, ICommingEventRepository
    {
        public CommingEventRepository(IDatabase database)
            : base(database)
        {
        }

        public CommingEventRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public IQueryable<ICommingEvent> GetAll()
        {
            return Database.CommingEventDataSource.OrderBy(r => r.EventDate);
        }

        public IQueryable<ICommingEvent> GetAllComming()
        {
            return Database.CommingEventDataSource.Where(x => x.EventDate.Date >= DateTime.Now.Date).OrderBy(x => x.EventDate);
        }
    }
}
