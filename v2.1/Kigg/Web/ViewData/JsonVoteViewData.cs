namespace Kigg.Web
{
    public class JsonVoteViewData : JsonViewData
    {
        public int votes
        {
            get;
            set;
        }

        public string text { get; set; }
    }
}