namespace Kigg.Web
{
    using System.Collections.Generic;

    public abstract class BaseStoryViewData : BaseViewData
    {
        public string PromoteText
        {
            get;
            set;
        }

        public string DemoteText
        {
            get;
            set;
        }

        public string CountText
        {
            get;
            set;
        }

        public ICollection<string> SocialServices
        {
            get;
            set;
        }
    }
}