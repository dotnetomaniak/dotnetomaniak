using System;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{
    public partial class CommingEvent : ICommingEvent
    {
        public void Add(string eventLink, string eventName)
        {
            Check.Argument.IsNotEmpty(eventLink, "EventLink");
            Check.Argument.IsNotEmpty(eventName, "EventName");            
        }
    }
}
