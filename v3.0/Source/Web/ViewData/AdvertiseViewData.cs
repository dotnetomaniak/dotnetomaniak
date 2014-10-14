using System;

namespace Kigg.Web.ViewData
{
    public class AdvertiseViewData
    {
        public string id { get; set; }
        public string recommendationLink { get; set; }
        public string recommendationTitle { get; set; }
        public string imageLink { get; set; }
        public string imageTitle { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string email { get; set; }
        public int position { get; set; }
        public bool notificationIsSent { get; set; }
        public string isBanner { get; set; }

        public AdvertiseViewData()
        {
            isBanner = "";
            notificationIsSent = false;
            position = 999;
        }
    }
}