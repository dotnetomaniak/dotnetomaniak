namespace Kigg.Web
{
    public class JsonContentViewData : JsonViewData
    {
        public string title
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public bool alreadyExists
        {
            get;
            set;
        }

        public string existingUrl
        {
            get;
            set;
        }
    }
}