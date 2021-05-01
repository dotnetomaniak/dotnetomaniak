using Kigg.Core.DomainObjects;
using Kigg.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kigg.Core.Service.GoogleService
{
    public interface IGoogleService
    {
        string EventApproved(CommingEvent commingEvent);
        string EditEvent(CommingEvent commingEvent);
        void DeleteEvent(string id);
    }
}
