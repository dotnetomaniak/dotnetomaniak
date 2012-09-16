namespace Kigg.Service
{
    public interface IUserScoreTable
    {
        decimal AccountActivated
        {
            get;
        }

        decimal StorySubmitted
        {
            get;
        }

        decimal StoryViewed
        {
            get;
        }

        decimal UpcomingStoryPromoted
        {
            get;
        }

        decimal PublishedStoryPromoted
        {
            get;
        }

        decimal StoryPublished
        {
            get;
        }

        decimal StoryCommented
        {
            get;
        }

        decimal StoryMarkedAsSpam
        {
            get;
        }

        decimal SpamStorySubmitted
        {
            get;
        }

        decimal StoryIncorrectlyMarkedAsSpam
        {
            get;
        }

        decimal SpamCommentSubmitted
        {
            get;
        }
    }
}