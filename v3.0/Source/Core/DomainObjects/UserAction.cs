namespace Kigg.DomainObjects
{
    public enum UserAction
    {
        None = 0,
        AccountActivated = 1,
        StorySubmitted = 2,
        StoryViewed = 3,
        UpcomingStoryPromoted = 4,
        UpcomingStoryDemoted = 5,
        PublishedStoryPromoted = 6,
        PublishedStoryDemoted = 7,
        StoryCommented = 8,
        StoryPublished = 9,
        StoryMarkedAsSpam = 10,
        StoryUnmarkedAsSpam = 11,
        SpamStorySubmitted = 12,
        StoryIncorrectlyMarkedAsSpam = 13,
        SpamCommentSubmitted = 14,
        CommentMarkedAsOffended = 15,
        StoryDeleted = 16
    }
}