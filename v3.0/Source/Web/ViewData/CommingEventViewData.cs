using System;

namespace Kigg.Web.ViewData
{
    public class CommingEventViewData : BaseViewData
    {
        public string EventLink { get; set; }
        public string EventName { get; set; }        
        public string Id { get; set; }        
        public string GoogleEventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public string EventLead { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsOnline { get; set; }
        
        public bool IsEventTerminated()
        {
            if (EventDate.Date >= DateTime.Now.Date)            
                return false;            
            return true;
        }
    }
}