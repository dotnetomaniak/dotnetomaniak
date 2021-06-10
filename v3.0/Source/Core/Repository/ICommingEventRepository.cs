using Kigg.Core.DomainObjects;
using System;
using System.Linq;

namespace Kigg.Repository
{
    public interface ICommingEventRepository : IRepository<ICommingEvent>
    {
        IQueryable<ICommingEvent> GetAll();
        IQueryable<ICommingEvent> GetAllApproved();
        ICommingEvent FindById(Guid id);
        void EditEvent(ICommingEvent commingEvent, string eventUserEmail, string eventLink, string googleEventId, string eventName, DateTime eventDate, DateTime eventEndDate, string eventCity, string eventPlace,
            string eventLead, bool isApproved, bool isOnline);

        int CountByUnapproved();
    }
}
