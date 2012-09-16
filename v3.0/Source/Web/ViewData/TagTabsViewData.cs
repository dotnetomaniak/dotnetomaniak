namespace Kigg.Web
{
    using System.Collections.Generic;

    using DomainObjects;

    public class TagTabsViewData
    {
        public ICollection<ITag> PopularTags
        {
            get;
            set;
        }

        public ICollection<ITag> UserTags
        {
            get;
            set;
        }
    }
}