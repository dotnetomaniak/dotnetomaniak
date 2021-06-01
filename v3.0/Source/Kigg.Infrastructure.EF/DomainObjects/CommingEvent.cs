using Kigg.Core.DomainObjects;
using System;

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

        public string GoogleEventId { get; set; }

        public DateTime? EventEndDate { get; set; }

        public string EventCity { get; set; }

        public bool? IsOnline { get; set; }
    }
}