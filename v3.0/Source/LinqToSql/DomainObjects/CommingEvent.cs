using System;
using Kigg.Core.DomainObjects;
using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{
    public partial class CommingEvent : ICommingEvent
    {
        public void Add(string eventLink, string eventName, DateTime eventDate, string imageLink, string imageTitle, DateTime createdAt, DateTime startTime, DateTime endTime, int position)
        {
            Check.Argument.IsNotEmpty(eventLink, "EventLink");
            Check.Argument.IsNotEmpty(eventName, "EventName");            
            Check.Argument.IsNotEmpty(imageLink, "ImageLink");
            Check.Argument.IsNotEmpty(imageTitle, "ImageTitle");
        }
    }
}
