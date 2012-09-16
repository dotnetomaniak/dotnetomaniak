using System;

namespace Kigg.Infrastructure
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using DomainObjects;
    using Service;

    public class SendTwitterMessage : BaseBackgroundTask
    {
        private const int MaxLength = 140;

        private readonly IContentService _contentService;
        private readonly IHttpForm _httpForm;

        private readonly string _userName;
        private readonly string _password;
        private readonly string _source;
        private readonly string _statusUrl;
        private readonly string _directMessageUrl;
        private readonly string[] _directMessageRecipients;
        private readonly bool _useOriginalUrlOfStory;

        private SubscriptionToken _storySubmitToken;
        private SubscriptionToken _storyApproveToken;
        private SubscriptionToken _storyPublishToken;
        private SubscriptionToken _possibleSpamStoryToken;
        private SubscriptionToken _possibleSpamCommentToken;
        private SubscriptionToken _storyMarkAsSpamToken;

        public SendTwitterMessage(IEventAggregator eventAggregator, IContentService contentService, IHttpForm httpForm, string userName, string password, string source, string statusUrl, bool useOriginalUrlOfStory, string directMessageUrl, string directMessageRecipients) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotNull(httpForm, "httpForm");
            Check.Argument.IsNotNull(userName, "userName");
            Check.Argument.IsNotNull(password, "password");
            Check.Argument.IsNotNull(source, "source");
            Check.Argument.IsNotEmpty(statusUrl, "statusUrl");
            Check.Argument.IsNotEmpty(directMessageUrl, "directMessageUrl");
            Check.Argument.IsNotEmpty(directMessageRecipients, "directMessageRecipients");

            _contentService = contentService;
            _httpForm = httpForm;

            _userName = userName;
            _password = password;
            _source = source;
            _statusUrl = statusUrl;
            _useOriginalUrlOfStory = useOriginalUrlOfStory;
            _directMessageUrl = directMessageUrl;
            _directMessageRecipients = directMessageRecipients.Split('|');
        }

        protected override void OnStart()
        {
            if (!IsRunning)
            {
                _storySubmitToken = Subscribe<StorySubmitEvent, StorySubmitEventArgs>(StorySubmitted);
                _storyApproveToken = Subscribe<StoryApproveEvent, StoryApproveEventArgs>(StoryApproved);
                _storyPublishToken = Subscribe<StoryPublishEvent, StoryPublishEventArgs>(StoryPublished);
                _possibleSpamStoryToken = Subscribe<PossibleSpamStoryEvent, PossibleSpamStoryEventArgs>(PossibleSpamStoryDetected);
                _possibleSpamCommentToken = Subscribe<PossibleSpamCommentEvent, PossibleSpamCommentEventArgs>(PossibleSpamCommentDetected);
                _storyMarkAsSpamToken = Subscribe<StoryMarkAsSpamEvent, StoryMarkAsSpamEventArgs>(StoryMarkedAsSpam);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<StorySubmitEvent>(_storySubmitToken);
                Unsubscribe<StoryApproveEvent>(_storyApproveToken);
                Unsubscribe<StoryPublishEvent>(_storyPublishToken);
                Unsubscribe<PossibleSpamStoryEvent>(_possibleSpamStoryToken);
                Unsubscribe<PossibleSpamCommentEvent>(_possibleSpamCommentToken);
                Unsubscribe<StoryMarkAsSpamEvent>(_storyMarkAsSpamToken);
            }
        }

        internal void StorySubmitted(StorySubmitEventArgs eventArgs)
        {
            UpdateStatusForStorySubmit(eventArgs.Story, eventArgs.DetailUrl);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            UpdateStatusForStorySubmit(eventArgs.Story, eventArgs.DetailUrl);
        }

        internal void StoryPublished(StoryPublishEventArgs eventArgs)
        {
            foreach(PublishedStory publishedStory in eventArgs.PublishedStories)
            {
                UpdateStatusForStoryPublish(publishedStory.Story);
            }
        }

        internal void PossibleSpamStoryDetected(PossibleSpamStoryEventArgs eventArgs)
        {
            const string Prefix = "POSSIBLE SPAM STORY: ";

            string message = BuildMessage(Prefix, eventArgs.DetailUrl, eventArgs.Story.Title);

            SendMessage(message);
        }

        internal void PossibleSpamCommentDetected(PossibleSpamCommentEventArgs eventArgs)
        {
            const string Prefix = "POSSIBLE SPAM COMMENT: ";

            string message = BuildMessage(Prefix, eventArgs.DetailUrl, eventArgs.Comment.ForStory.Title);

            SendMessage(message);
        }

        internal void StoryMarkedAsSpam(StoryMarkAsSpamEventArgs eventArgs)
        {
            const string Prefix = "STORY MARKED AS SPAM: ";

            string message = BuildMessage(Prefix, eventArgs.DetailUrl, eventArgs.Story.Title);

            SendMessage(message);
        }

        private void UpdateStatusForStorySubmit(IStory story, string storyUrl)
        {
            const string Prefix = "NEW: ";

            string status = BuildStatus(Prefix, story, storyUrl);

            UpdateStatus(status);
        }

        private void UpdateStatusForStoryPublish(IStory story)
        {
            const string Prefix = "PUBLISHED: ";

            string status = BuildStatus(Prefix, story, story.Url);

            UpdateStatus(status);
        }

        private void UpdateStatus(string status)
        {
            var postRequest = new HttpFormPostRequest
                                  {
                                      Url = _statusUrl,
                                      UserName = _userName,
                                      Password = _password,
                                      FormFields = new NameValueCollection
                                                       {
                                                           {"status", status.UrlEncode()}
                                                       }
                                  };

            if (!string.IsNullOrEmpty(_source))
            {
                postRequest.FormFields.Add("source", _source.UrlEncode());
            }

            _httpForm.PostAsync(postRequest);
        }

        private void SendMessage(string message)
        {
            foreach(string recipient in _directMessageRecipients)
            {
                var postRequest = new HttpFormPostRequest
                                      {
                                          Url = _directMessageUrl,
                                          UserName = _userName,
                                          Password = _password,
                                          FormFields = new NameValueCollection
                                                           {
                                                               {"user", recipient.UrlEncode()},
                                                               {"text", message.UrlEncode()}
                                                           }
                                      };

                _httpForm.PostAsync(postRequest);
            }
        }

        private string BuildStatus(string prefix, IStory story, string storyUrl)
        {
            var statusBuilder = new StringBuilder();

            statusBuilder.Append(BuildMessage(prefix, _useOriginalUrlOfStory ? story.Url : storyUrl, story.Title));

            if (statusBuilder.Length < MaxLength)
            {
                for (int i = 0; i < story.Tags.Count; i++)
                {
                    string tagName = (i == 0 ? "," : " ") + "#" + story.Tags.ElementAt(i).Name.Replace(" ",String.Empty); 
                    if ((statusBuilder.Length + tagName.Length) < MaxLength)
                    {
                        statusBuilder.Append(tagName);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return statusBuilder.ToString();
        }

        private string BuildMessage(string prefix, string storyUrl, string storyTitle)
        {
            string shortUrl = _contentService.ShortUrl(storyUrl);
            int wrapLength = MaxLength - (prefix.Length + shortUrl.Length + 3);

            string message = "{0}{1} - {2}".FormatWith(prefix, storyTitle.WrapAt(wrapLength), shortUrl);

            return message;
        }
    }
}