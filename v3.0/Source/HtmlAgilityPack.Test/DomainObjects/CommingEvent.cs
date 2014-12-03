using Kigg.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.LinqToSql.DomainObjects
{
    public partial class CommingEvent : ICommingEvent
    {
        public void Add(string eventLink, string eventName, DateTime eventDate, string eventPlace, string eventLead)
        {
            Check.Argument.IsNotEmpty(eventLink, "EventLink");
            Check.Argument.IsNotEmpty(eventName, "EventName");
            Check.Argument.IsNotEmpty(eventDate, "EventDate");
            Check.Argument.IsNotEmpty(eventPlace, "EventPlace");
            Check.Argument.IsNotEmpty(eventLead, "EventLead");
        }
    }
}
