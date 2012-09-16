namespace Kigg.Service
{
    public class UserScoreTable : IUserScoreTable
    {
        public decimal AccountActivated
        {
            get;
            set;
        }

        public decimal StorySubmitted
        {
            get;
            set;
        }

        public decimal StoryViewed
        {
            get;
            set;
        }

        public decimal UpcomingStoryPromoted
        {
            get;
            set;
        }

        public decimal PublishedStoryPromoted
        {
            get;
            set;
        }

        public decimal StoryPublished
        {
            get;
            set;
        }

        public decimal StoryCommented
        {
            get;
            set;
        }

        public decimal StoryMarkedAsSpam
        {
            get;
            set;
        }

        public decimal SpamStorySubmitted
        {
            get;
            set;
        }

        public decimal StoryIncorrectlyMarkedAsSpam
        {
            get;
            set;
        }

        public decimal SpamCommentSubmitted
        {
            get;
            set;
        }
    }
}