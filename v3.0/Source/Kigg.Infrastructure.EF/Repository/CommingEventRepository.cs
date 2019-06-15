using System;
using System.Linq;
using Kigg.Core.DomainObjects;
using Kigg.Infrastructure.EF.DomainObjects;
using Kigg.Repository;

namespace Kigg.Infrastructure.EF.Repository
{
    public class CommingEventRepository : BaseRepository<CommingEvent>, ICommingEventRepository
    {
        private readonly DotnetomaniakContext _context;

        public CommingEventRepository(DotnetomaniakContext context) : base(context)
        {
            _context = context;
        }

        public void Add(ICommingEvent entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            CommingEvent commingEvent = (CommingEvent) entity;

            base.Add(commingEvent);
        }

        public void Remove(ICommingEvent entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            CommingEvent commingEvent = (CommingEvent) entity;

            base.Remove(commingEvent);
        }

        public IQueryable<ICommingEvent> GetAll()
        {
            return _context.CommingEvents.OrderBy(r => r.EventDate);
        }

        public IQueryable<ICommingEvent> GetAllApproved()
        {
            return _context.CommingEvents
                .Where(x => x.IsApproved.GetValueOrDefault())
                .OrderBy(x => x.EventDate);
        }

        public ICommingEvent FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");
            return _context.CommingEvents.SingleOrDefault(x => x.Id == id);
        }


        public void EditEvent(ICommingEvent commingEvent, string eventUserEmail, string eventLink, string eventName,
            DateTime eventDate, string eventPlace,
            string eventLead, bool isApproved)
        {
            Check.Argument.IsNotNull(commingEvent, "CommingEvent");

            if (!string.IsNullOrEmpty(eventUserEmail)) commingEvent.Email = eventUserEmail;
            if (!string.IsNullOrEmpty(eventLink)) commingEvent.EventLink = eventLink;
            if (!string.IsNullOrEmpty(eventName)) commingEvent.EventName = eventName;

            commingEvent.EventDate = eventDate;
            commingEvent.EventPlace = eventPlace;
            commingEvent.EventLead = eventLead;
            commingEvent.Email = eventUserEmail;
            commingEvent.IsApproved = isApproved;
        }

        public int CountByUnapproved()
        {
            return _context.CommingEvents.Count(x => x.IsApproved == false);
        }
    }
}