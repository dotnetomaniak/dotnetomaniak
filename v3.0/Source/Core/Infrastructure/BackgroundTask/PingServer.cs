namespace Kigg.Infrastructure
{
    using System;
    using Service;

    public class PingServer : BaseBackgroundTask
    {
        private readonly IConfigurationSettings _settings;
        private readonly IHttpForm _httpForm;

        private readonly string _url;
        private readonly float _intervalInMinutes;

        private DateTime _lastPingedAt;

        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyApproveToken;

        public PingServer(IConfigurationSettings settings, IEventAggregator eventAggregator, IHttpForm httpForm, string url, float intervalInMinutes) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(httpForm, "httpForm");
            Check.Argument.IsNotEmpty(url, "url");
            Check.Argument.IsNotNegative(intervalInMinutes, "intervalInMinutes");

            _settings = settings;
            _httpForm = httpForm;
            _url = url;
            _intervalInMinutes = intervalInMinutes;
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
            SendPing();
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            SendPing();
        }

        private void SendPing()
        {
            const string RequestXml =   "<?xml version=\"1.0\"?>" +
                                        "<methodCall>" +
                                            "<methodName>weblogUpdates.ping</methodName>" +
                                            "<params>" +
                                                "<param>" +
                                                    "<value>{0}</value>" +
                                                "</param>" +
                                                "<param>" +
                                                    "<value>{0}</value>" +
                                                "</param>" +
                                            "</params>" +
                                        "</methodCall>";
            if (ShouldPing())
            {
                _httpForm.PostAsync(
                                        new HttpFormPostRawRequest
                                        {
                                            Url = _url,
                                            ContentType = "text/xml",
                                            Data = RequestXml.FormatWith(_settings.SiteTitle, _settings.RootUrl)
                                        },
                                        r => _lastPingedAt = SystemTime.Now(),
                                        e => { }
                                    );
            }
        }

        private bool ShouldPing()
        {
            return (SystemTime.Now() - _lastPingedAt).TotalMinutes >= _intervalInMinutes;
        }
    }
}