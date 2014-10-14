using Kigg.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.Infrastructure.LinqToSql.DomainObjects
{
    public partial class CommingEvent : ICommingEvent
    {
        public void Add(string eventLink, string eventName, DateTime eventDate)
        {
            Check.Argument.IsNotEmpty(eventLink, "EventLink");
            Check.Argument.IsNotEmpty(eventName, "EventName");
            Check.Argument.IsNotEmpty(eventDate, "EventDate");
        }
    }
}
