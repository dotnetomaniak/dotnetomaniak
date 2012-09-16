namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;

    using DomainObjects;
    using Service;

    public class PingStory : BaseBackgroundTask
    {
        private readonly IConfigurationSettings _settings;
        private readonly IHttpForm _httpForm;
        private readonly IContentService _contentService;
        private readonly string _textFormat;

        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyApproveToken;

        public PingStory(IConfigurationSettings settings, IEventAggregator eventAggregator, IHttpForm httpForm, IContentService contentService, string textFormat): base(eventAggregator)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(httpForm, "httpForm");
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotEmpty(textFormat, "textFormat");

            _settings = settings;
            _httpForm = httpForm;
            _contentService = contentService;
            _textFormat = textFormat;
        }

        protected override void OnStart()
        {
            if (_settings.SendPing && !IsRunning)
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
            SendPing(eventArgs.Story, eventArgs.DetailUrl);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            SendPing(eventArgs.Story, eventArgs.DetailUrl);
        }

        private void SendPing(IStory story, string detailUrl)
        {
            StoryContent content = _contentService.Get(story.Url);

            if (!string.IsNullOrEmpty(content.TrackBackUrl))
            {
                HttpFormPostRequest request = new HttpFormPostRequest
                                                  {
                                                      Url = content.TrackBackUrl,
                                                      FormFields = new NameValueCollection
                                                                   {
                                                                       {"title", story.Title.UrlEncode() },
                                                                       {"url", detailUrl.UrlEncode() },
                                                                       {"excerpt", _textFormat.FormatWith(_settings.SiteTitle).UrlEncode() },
                                                                       {"blog_name", _settings.SiteTitle.UrlEncode() }
                                                                   }
                                                  };
                _httpForm.PostAsync(request);
            }
        }
    }
}