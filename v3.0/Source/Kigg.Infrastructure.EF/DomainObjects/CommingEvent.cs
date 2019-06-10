using Kigg.Core.DomainObjects;

namespace Kigg.Infrastructure.EF.DomainObjects
{
    public class CommingEvent: Entity, ICommingEvent
    {
        public string EventLink { get; set; }

        public string EventName { get; set; }

        public System.DateTime EventDate { get; set; }

        public string EventPlace { get; set; }

        public string EventLead { get; set; }

        public string Email { get; set; }

        public bool? IsApproved { get; set; }
    }
}