namespace Kigg.Web
{
    using System.Collections.Generic;

    public class TopUserTabsViewData
    {
        public ICollection<UserWithScore> TopMovers
        {
            get;
            set;
        }

        public ICollection<UserWithScore> TopLeaders
        {
            get;
            set;
        }
    }
}