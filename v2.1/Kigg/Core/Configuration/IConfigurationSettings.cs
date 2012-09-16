namespace Kigg
{
    public interface IConfigurationSettings
    {
        string RootUrl
        {
            get;
        }

        string WebmasterEmail
        {
            get;
        }

        string SupportEmail
        {
            get;
        }

        string DefaultEmailOfOpenIdUser
        {
            get;
        }

        string SiteTitle
        {
            get;
        }

        string MetaKeywords
        {
            get;
        }

        string MetaDescription
        {
            get;
        }

        int TopTags
        {
            get;
        }

        int HtmlStoryPerPage
        {
            get;
        }

        int FeedStoryPerPage
        {
            get;
        }

        int CarouselStoryCount
        {
            get;
        }

        int HtmlUserPerPage
        {
            get;
        }

        int TopUsers
        {
            get;
        }

        bool AutoDiscoverContent
        {
            get;
        }

        bool SendPing
        {
            get;
        }

        string PromoteText
        {
            get;
        }

        string DemoteText
        {
            get;
        }

        string CountText
        {
            get;
        }

        float MinimumAgeOfStoryInHoursToPublish
        {
            get;
        }

        float MaximumAgeOfStoryInHoursToPublish
        {
            get;
        }

        bool AllowPossibleSpamStorySubmit
        {
            get;
        }

        bool SendMailWhenPossibleSpamStorySubmitted
        {
            get;
        }

        bool AllowPossibleSpamCommentSubmit
        {
            get;
        }

        bool SendMailWhenPossibleSpamCommentSubmitted
        {
            get;
        }

        string PublishedStoriesFeedBurnerUrl
        {
            get;
        }

        string UpcomingStoriesFeedBurnerUrl
        {
            get;
        }

        decimal MaximumUserScoreToShowCaptcha
        {
            get;
        }

        int StorySumittedThresholdOfUserToSpamCheck
        {
            get;
        }
    }
}