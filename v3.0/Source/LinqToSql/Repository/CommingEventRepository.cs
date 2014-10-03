using Kigg.Infrastructure.LinqToSql.DomainObjects;
using Kigg.LinqToSql.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kigg.Repository;
using Kigg.Core.Repository;
using Kigg.Core.DomainObjects;

namespace Kigg.Infrastructure.LinqToSql.Repository
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

        public IQueryable<IRecommendation> GetAll()
        {
            return Database.CommingEventDataSource.OrderBy(r => r.Position);
        }
    }
}
