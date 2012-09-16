using System.Collections.Generic;

namespace Kigg.Web
{
    public class BadgesViewData : BaseViewData
    {
        public IEnumerable<BadgeViewData> Badges { get; set; }
    }

    public class BadgeViewData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; } 
    }
}