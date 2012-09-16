namespace Kigg.Infrastructure
{
    using System.Diagnostics;

    public class StoryContent
    {
        private static readonly StoryContent _empty = new StoryContent(null, null, null);

        private readonly string _title;
        private readonly string _description;
        private readonly string _trackBackUrl;

        public StoryContent(string title, string description, string trackBackUrl)
        {
            _title = title;
            _description = description;
            _trackBackUrl = trackBackUrl;
        }

        public static StoryContent Empty
        {
            [DebuggerStepThrough]
            get
            {
                return _empty;
            }
        }

        public string Title
        {
            [DebuggerStepThrough]
            get
            {
                return _title;
            }
        }

        public string Description
        {
            [DebuggerStepThrough]
            get
            {
                return _description;
            }
        }

        public string TrackBackUrl
        {
            [DebuggerStepThrough]
            get
            {
                return _trackBackUrl;
            }
        }
    }
}