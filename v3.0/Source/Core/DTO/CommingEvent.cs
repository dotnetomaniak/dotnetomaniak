using System;

namespace Kigg.Core.DTO
{
    public class CommingEvent
    {        
        public string Id { get; set; }
        public string EventLink { get; set; }
        public string EventName { get; set; }
        public string GoogleEventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventCity { get; set; }
        public string EventPlace { get; set; }
        public string EventLead { get; set; }
        public string EventUserEmail { get; set; }
        public bool IsApproved { get; set; }
        public bool IsOnline { get; set; }

        public CommingEvent(string name, string link, string googleEventId, DateTime date, DateTime endDate, string city, string place, string lead, bool isOnline)
        {
            EventName = name;
            EventLink = link;
            GoogleEventId = googleEventId;
            EventDate = date;
            EventEndDate = endDate;
            EventCity = city;
            EventPlace = place;
            EventLead = lead;
            IsOnline = isOnline;
        }
    }
}