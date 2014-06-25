namespace Kigg
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        public string RootUrl
        {
            get;
            set;
        }

        public string WebmasterEmail
        {
            get;
            set;
        }

        public string SupportEmail
        {
            get;
            set;
        }

        public string DefaultEmailOfOpenIdUser
        {
            get;
            set;
        }

        public string SiteTitle
        {
            get;
            set;
        }

        public string MetaKeywords
        {
            get;
            set;
        }

        public string MetaDescription
        {
            get;
            set;
        }

        public int TopTags
        {
            get;
            set;
        }

        public int HtmlStoryPerPage
        {
            get;
            set;
        }

        public int FeedStoryPerPage
        {
            get;
            set;
        }

        public int CarouselStoryCount
        {
            get;
            set;
        }

        public int HtmlUserPerPage
        {
            get;
            set;
        }

        public int TopUsers
        {
            get;
            set;
        }

        public bool AutoDiscoverContent
        {
            get;
            set;
        }

        public bool SendPing
        {
            get;
            set;
        }

        public string PromoteText
        {
            get;
            set;
        }

        public string PromoteTextForCounter { get; set; }

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

        public float MinimumAgeOfStoryInHoursToPublish
        {
            get;
            set;
        }

        public float MaximumAgeOfStoryInHoursToPublish
        {
            get;
            set;
        }

        public bool AllowPossibleSpamStorySubmit
        {
            get;
            set;
        }

        public bool SendMailWhenPossibleSpamStorySubmitted
        {
            get;
            set;
        }

        public bool AllowPossibleSpamCommentSubmit
        {
            get;
            set;
        }

        public bool SendMailWhenPossibleSpamCommentSubmitted
        {
            get;
            set;
        }

        public string PublishedStoriesFeedBurnerUrl
        {
            get;
            set;
        }

        public string UpcomingStoriesFeedBurnerUrl
        {
            get;
            set;
        }

        public decimal MaximumUserScoreToShowCaptcha
        {
            get;
            set;
        }

        public int StorySumittedThresholdOfUserToSpamCheck
        {
            get;
            set;
        }

        public string FacebookAppId
        {
            get;
            set;
        }
    }
}