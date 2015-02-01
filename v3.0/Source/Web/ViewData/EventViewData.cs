using System;

namespace Kigg.Web.ViewData
{
    public class EventViewData
    {        
        public string Id { get; set; }
        public string EventLink { get; set; }
        public string EventName { get; set; }        
        public DateTime EventDate { get; set; }
        public string EventPlace { get; set; }
        public string EventLead { get; set; }
        public string EventUserEmail { get; set; }
        public bool IsApproved { get; set; }
    }
}