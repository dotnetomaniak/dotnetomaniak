using System.Collections.Generic;
namespace Kigg.Web
{
    public class PromoteItem
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }
    public class PromoteSiteViewData : BaseViewData
    {
        public List<PromoteItem> Items { get; set; }
    }
}
