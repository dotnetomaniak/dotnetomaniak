using Kigg.Service;

namespace Kigg.Infrastructure
{
    public class SlackIntegration : BaseBackgroundTask
    {
        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyApproveToken;
        private SubscriptionToken _upcommingEventToken;
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
                _upcommingEventToken = Subscribe<UpcommingEventEvent, UpcommingEventEventArgs>(EventAdded);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<StorySubmitEvent>(_storySubmitToken);
                Unsubscribe<StoryApproveEvent>(_storyApproveToken);
                Unsubscribe<UpcommingEventEvent>(_upcommingEventToken);
            }
        }

        internal void EventAdded(UpcommingEventEventArgs eventArgs)
        {
            string content = "Dodano wydarzenie - <{0}|{1}>".FormatWith(eventArgs.EventName, eventArgs.EventLink);
            SendSlackNotification(content);
        }

        internal void StorySubmitted(StorySubmitEventArgs eventArgs)
        {
            string content = "Dodano artykuł - <{0}|{1}>".FormatWith(eventArgs.Story.Title, eventArgs.DetailUrl);
            SendSlackNotification(content);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            string content = "Dodano artykuł - <{0}|{1}>".FormatWith(eventArgs.Story.Title, eventArgs.DetailUrl);
            SendSlackNotification(content);
        }

        private void SendSlackNotification(string content)
        {
            
            _httpForm.Post(new HttpFormPostRawRequest
                {
                    Url = _uniqueUrl,
                    Data = @"{{""text"": ""{0}""}}".FormatWith(content),
                    ContentType = "application/json"
                });
        }
    }
}
