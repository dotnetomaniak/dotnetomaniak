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

        public IQueryable<ICommingEvent> GetAll()
        {
            return Database.CommingEventDataSource.OrderBy(r => r.EventDate);
        }

        public IQueryable<ICommingEvent> GetAllComming()
        {
            return Database.CommingEventDataSource.Where(x => x.EventDate.Date >= DateTime.Now.Date).OrderBy(x => x.EventDate);
        }

        public ICommingEvent FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");
            return Database.CommingEventDataSource.SingleOrDefault(x => x.Id == id);
        }


        public void EditEvent(ICommingEvent commingEvent, string eventLink, string eventName, DateTime eventDate, string eventPlace, string eventLead)
        {
            Check.Argument.IsNotNull(commingEvent, "CommingEvent");

            if (!string.IsNullOrEmpty(eventLink)) commingEvent.EventLink = eventLink;
            if (!string.IsNullOrEmpty(eventName)) commingEvent.EventName = eventName;

            commingEvent.EventDate = eventDate;
            commingEvent.EventPlace = eventPlace;
            commingEvent.EventLead = eventLead;
        }
    }
}
