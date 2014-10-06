using Kigg.Service;

namespace Kigg.Infrastructure
{
    public class SlackIntegration : BaseBackgroundTask
    {
        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyApproveToken;
        private readonly IHttpForm _httpForm;
        private readonly string _uniqueUrl;

        public SlackIntegration(IEventAggregator eventAggregator, IHttpForm httpForm, string uniqueUrl) : base(eventAggregator)
        {
            Check.Argument.IsNotEmpty(uniqueUrl, "uniqueUrl");
            Check.Argument.IsNotNull(httpForm, "httpForm");

            _httpForm = httpForm;
            _uniqueUrl = uniqueUrl;
        }

        protected override void OnStart()
        {
            if (!IsRunning)
            {
                _storySubmitToken = Subscribe<StorySubmitEvent, StorySubmitEventArgs>(StorySubmitted);
                _storyApproveToken = Subscribe<StoryApproveEvent, StoryApproveEventArgs>(StoryApproved);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<StorySubmitEvent>(_storySubmitToken);
                Unsubscribe<StoryApproveEvent>(_storyApproveToken);
            }
        }

        internal void StorySubmitted(StorySubmitEventArgs eventArgs)
        {
            SendSlackNotification(eventArgs.Story.Title, eventArgs.DetailUrl);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            SendSlackNotification(eventArgs.Story.Title, eventArgs.DetailUrl);
        }

        private void SendSlackNotification(string title, string url)
        {
            string content = "Dodano artykuł - <{0}|{1}>".FormatWith(url, title);
            _httpForm.Post(new HttpFormPostRawRequest
                {
                    Url = _uniqueUrl,
                    Data = @"{{""text"": ""{0}""}}".FormatWith(content),
                    ContentType = "application/json"
                });
        }
    }
}
