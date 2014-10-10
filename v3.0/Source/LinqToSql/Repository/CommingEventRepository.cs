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

        public override void Add(ICommingEvent entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            CommingEvent commingEvent = (CommingEvent)entity;

            base.Add(commingEvent);
        }

        public override void Remove(ICommingEvent entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            CommingEvent commingEvent = (CommingEvent)entity;

            base.Remove(commingEvent);
        }

        public ICommingEvent FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.CommingEventDataSource.SingleOrDefault(s => s.Id == id);
        }

        public ICommingEvent FindByCommingEventName(string commingEventName)
        {
            Check.Argument.IsNotEmpty(commingEventName, "commingEventName");

            return Database.CommingEventDataSource.SingleOrDefault(r => r.EventName == commingEventName.Trim());
        }

        public IQueryable<ICommingEvent> GetAll()
        {
            return Database.CommingEventDataSource.OrderBy(r => r.Position);
        }

        public IQueryable<ICommingEvent> GetAllVisible()
        {
            var now = SystemTime.Now();

            return Database.CommingEventDataSource                
                .Where(r => r.StartTime < now && r.EndTime >= now)
                .OrderBy(r => r.Position);
        }
    }
}
