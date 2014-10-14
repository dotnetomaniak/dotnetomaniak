using Kigg.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kigg.Core.DomainObjects
{
    public partial interface ICommingEvent
    {
        Guid Id
        {
            get;
        }

        string EventLink
        {
            get;
            set;
        }

        string EventName
        {
            get;
            set;
        }

        DateTime EventDate
        {
            get;
            set;
        }
    }
}
