using System;
using System.Collections.Generic;
namespace Kigg.Web
{    
    public class PromoteItem
    {
        public string Url { get; set; }
        public string Text { get; set; }

        public string Jpg { get; set; }
        public string Pdf { get; set; }
        public string Eps { get; set; }
        public string Png { get; set; }
    }

    public class PromoteSiteViewData : BaseViewData
    {
        public Dictionary<string, List<PromoteItem>> Items { get; set; }
    }
}
