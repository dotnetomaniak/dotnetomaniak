using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kigg.Web.ViewData
{
    public class RecommendationViewData : BaseViewData
    {
        public string UrlLink { get; set; }
        public string UrlTitle { get; set; }
        public string ImageName { get; set; }
        public string ImageAlt { get; set; }
    }
}