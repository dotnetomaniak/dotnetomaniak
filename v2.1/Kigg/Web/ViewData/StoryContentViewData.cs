namespace Kigg.Web
{
    public class StoryContentViewData : BaseViewData
    {
        public string Url
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool AutoDiscover
        {
            get;
            set;
        }

        public bool CaptchaEnabled
        {
            get;
            set;
        }
    }
}