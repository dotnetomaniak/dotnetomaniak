namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class StoryService : IStoryService
    {
        private readonly IConfigurationSettings _settings;
        private readonly IUserScoreService _userScoreService;
        private readonly IDomainObjectFactory _factory;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IMarkAsSpamRepository _markAsSpamRepository;
        private readonly ISpamProtection _spamProtection;
        private readonly ISpamPostprocessor _spamPostprocessor;
        private readonly IEmailSender _emailSender;
        private readonly IContentService _contentService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IThumbnail _thumbnail;

        private readonly IStoryWeightCalculator[] _storyWeightCalculators;

        public StoryService(IConfigurationSettings settings, IUserScoreService userScoreService, IDomainObjectFactory factory, ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository, IMarkAsSpamRepository markAsSpamRepository, ISpamProtection spamProtection, ISpamPostprocessor spamPostprocessor, IEmailSender emailSender, IContentService contentService, IHtmlSanitizer htmlSanitizer, IThumbnail thumbnail, IStoryWeightCalculator[] storyWeightCalculators)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(userScoreService, "userScoreService");
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(markAsSpamRepository, "markAsSpamRepository");
            Check.Argument.IsNotNull(spamProtection, "spamProtection");
            Check.Argument.IsNotNull(spamPostprocessor, "spamPostprocessor");
            Check.Argument.IsNotNull(emailSender, "emailSender");
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotNull(htmlSanitizer, "htmlSanitizer");
            Check.Argument.IsNotNull(thumbnail, "thumbnail");
            Check.Argument.IsNotEmpty(storyWeightCalculators, "storyWeightCalculators");

            _settings = settings;
            _userScoreService = userScoreService;
            _factory = factory;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _storyRepository = storyRepository;
            _markAsSpamRepository = markAsSpamRepository;
            _spamProtection = spamProtection;
            _spamPostprocessor = spamPostprocessor;
            _emailSender = emailSender;
            _contentService = contentService;
            _htmlSanitizer = htmlSanitizer;
            _thumbnail = thumbnail;
            _storyWeightCalculators = storyWeightCalculators;
        }

        public virtual StoryCreateResult Create(IUser byUser, string url, string title, string category, string description, string tags, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables, Func<IStory, string> buildDetailUrl)
        {
            StoryCreateResult result = ValidateCreate(byUser, url, title, category, description, userIPAddress, userAgent);

            if (result == null)
            {
                using (IUnitOfWork unitOfWork = UnitOfWork.Get())
                {

                    IStory alreadyExists = _storyRepository.FindByUrl(url);

                    if (alreadyExists != null)
                    {
                        return new StoryCreateResult { ErrorMessage = "Wpis z takim samym url ju¿ istnieje.", DetailUrl = buildDetailUrl(alreadyExists) };
                    }

                    ICategory storyCategory = _categoryRepository.FindByUniqueName(category);

                    if (storyCategory == null)
                    {
                        return new StoryCreateResult { ErrorMessage = "\"{0}\" - kategoria nie istnieje.".FormatWith(category) };
                    }

                    StoryContent content = _contentService.Get(url);

                    if (content == StoryContent.Empty)
                    {
                        return new StoryCreateResult { ErrorMessage = "Podany url wydaje siê nie byæ poprawnym." };
                    }

                    description = _htmlSanitizer.Sanitize(description);

                    if (!_settings.AllowPossibleSpamStorySubmit && ShouldCheckSpamForUser(byUser))
                    {
                        result = EnsureNotSpam<StoryCreateResult>(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables, "Spamowy wpis odrzucony : {0}, {1}".FormatWith(url, byUser), "Twój wpis wydaje siê byæ spamem.");

                        if (result != null)
                        {
                            return result;
                        }
                    }

                    // If we are here which means story is not spam
                    IStory story = _factory.CreateStory(storyCategory, byUser, userIPAddress, title.StripHtml(), description, url);

                    _storyRepository.Add(story);

                    // The Initial vote;
                    story.Promote(story.CreatedAt, byUser, userIPAddress);

                    // Capture the thumbnail, might speed up the thumbnail generation process
                    _thumbnail.Capture(story.Url);

                    // Subscribe comments by default
                    story.SubscribeComment(byUser);

                    AddTagsToContainers(tags, new ITagContainer[] { story, byUser });

                    _userScoreService.StorySubmitted(byUser);

                    string detailUrl = buildDetailUrl(story);

                    if (_settings.AllowPossibleSpamStorySubmit && _settings.SendMailWhenPossibleSpamStorySubmitted && ShouldCheckSpamForUser(byUser))
                    {
                        unitOfWork.Commit();
                        _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables),
                                               (source, isSpam) => _spamPostprocessor.Process(source, isSpam, detailUrl, story));
                    }
                    else
                    {
                        story.Approve(SystemTime.Now());
                    }

                    // Ping the Story
                    PingStory(content, story, detailUrl);

                    result = new StoryCreateResult { NewStory = story, DetailUrl = detailUrl };
                }
            }

            return result;
        }

        public virtual void Update(IStory theStory, string uniqueName, DateTime createdAt, string title, string category, string description, string tags)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            if (string.IsNullOrEmpty(uniqueName))
            {
                uniqueName = theStory.UniqueName;
            }

            if (!createdAt.IsValid())
            {
                createdAt = theStory.CreatedAt;
            }

            theStory.ChangeNameAndCreatedAt(uniqueName, createdAt);

            if (!string.IsNullOrEmpty(title))
            {
                theStory.Title = title;
            }

            if ((!string.IsNullOrEmpty(category)) && (string.Compare(category, theStory.BelongsTo.UniqueName, StringComparison.OrdinalIgnoreCase) != 0))
            {
                ICategory storyCategory = _categoryRepository.FindByUniqueName(category);
                theStory.ChangeCategory(storyCategory);
            }

            if (!string.IsNullOrEmpty(description))
            {
                theStory.HtmlDescription = description.Trim();
            }

            AddTagsToContainers(tags, new[] { theStory });
        }

        public virtual void Delete(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            _userScoreService.StoryDeleted(theStory);
            _storyRepository.Remove(theStory);
            _emailSender.NotifyStoryDelete(theStory, byUser);
        }

        public virtual void View(IStory theStory, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (byUser != null)
            {
                _userScoreService.StoryViewed(theStory, byUser);
            }

            theStory.View(SystemTime.Now(), fromIPAddress);
        }

        public virtual void Promote(IStory theStory, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (theStory.Promote(SystemTime.Now(), byUser, fromIPAddress))
            {
                _userScoreService.StoryPromoted(theStory, byUser);
            }
        }

        public virtual void Demote(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (theStory.Demote(SystemTime.Now(), byUser))
            {
                _userScoreService.StoryDemoted(theStory, byUser);
            }
        }

        public virtual void MarkAsSpam(IStory theStory, string storyUrl, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (theStory.MarkAsSpam(SystemTime.Now(), byUser, fromIPAddress))
            {
                _userScoreService.StoryMarkedAsSpam(theStory, byUser);
                _emailSender.NotifyStoryMarkedAsSpam(storyUrl, theStory, byUser);
            }
        }

        public virtual void UnmarkAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (theStory.UnmarkAsSpam(SystemTime.Now(), byUser))
            {
                _userScoreService.StoryUnmarkedAsSpam(theStory, byUser);
            }
        }

        public virtual CommentCreateResult Comment(IStory forStory, string storyUrl, IUser byUser, string content, bool subscribe, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables)
        {
            CommentCreateResult result = ValidateComment(forStory, byUser, content, userIPAddress, userAgent);

            if (result == null)
            {
                content = SanitizeHtml(content);

                if (!_settings.AllowPossibleSpamCommentSubmit)
                {
                    result = EnsureNotSpam<CommentCreateResult>(byUser, userIPAddress, userAgent, storyUrl, urlReferer, content, "comment", serverVariables, "Possible spam rejected : {0}, {1}, {2}".FormatWith(storyUrl, forStory.Title, byUser), "Your comment appears to be a spam.");

                    if (result != null)
                    {
                        return result;
                    }
                }

                IComment comment = forStory.PostComment(content, SystemTime.Now(), byUser, userIPAddress);

                if (subscribe)
                {
                    forStory.SubscribeComment(byUser);
                }
                else
                {
                    forStory.UnsubscribeComment(byUser);
                }

                _userScoreService.StoryCommented(forStory, byUser);

                // Notify the Comment Subscribers that a new comment is posted
                _emailSender.SendComment(storyUrl, comment, forStory.Subscribers);

                if (_settings.AllowPossibleSpamCommentSubmit && _settings.SendMailWhenPossibleSpamCommentSubmitted)
                {
                    _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, storyUrl, urlReferer, content, "comment", serverVariables), (source, isSpam) => _spamPostprocessor.Process(source, isSpam, storyUrl, comment));
                }

                result = new CommentCreateResult();
            }

            return result;
        }

        public virtual void Spam(IStory theStory, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theStory.IsPublished())
            {
                _userScoreService.StorySpammed(theStory);
                _storyRepository.Remove(theStory);
                _emailSender.NotifyConfirmSpamStory(storyUrl, theStory, byUser);
            }
        }

        public virtual void Spam(IComment theComment, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");

            theComment.ForStory.DeleteComment(theComment);
            _userScoreService.CommentSpammed(theComment.ByUser);
            _emailSender.NotifyConfirmSpamComment(storyUrl, theComment, byUser);
        }

        public virtual void MarkAsOffended(IComment theComment, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theComment.IsOffended)
            {
                theComment.MarkAsOffended();
                _userScoreService.CommentMarkedAsOffended(theComment.ByUser);
                _emailSender.NotifyCommentAsOffended(storyUrl, theComment, byUser);
            }
        }

        public virtual void Publish()
        {
            DateTime currentTime = SystemTime.Now();

            IList<PublishedStory> publishableStories = GetPublishableStories(currentTime);

            if (!publishableStories.IsNullOrEmpty())
            {
                // First penalty the user for marking the story as spam;
                // It is obvious that the Moderator has already reviewed the story
                // before it gets this far.
                PenaltyUsersForIncorrectlyMarkingStoriesAsSpam(publishableStories);

                //Then Publish the stories
                PublishStories(currentTime, publishableStories);
            }
        }

        public virtual void Approve(IStory theStory, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theStory.IsApproved())
            {
                theStory.Approve(SystemTime.Now());
                _emailSender.NotifyStoryApprove(storyUrl, theStory, byUser);
            }
        }

        private static SpamCheckContent CreateSpamCheckContent(IUser user, string userIpAddress, string userAgent, string url, string referer, string content, string type, NameValueCollection serverVariables)
        {
            SpamCheckContent checkContentToCheck = new SpamCheckContent
                                             {
                                                 UserIPAddress = userIpAddress,
                                                 UserAgent = userAgent,
                                                 UserName = user.UserName,
                                                 Url = url,
                                                 UrlReferer = referer,
                                                 Content = content,
                                                 ContentType = type
                                             };

            if (serverVariables != null)
            {
                checkContentToCheck.Extra.Add(serverVariables);
            }

            return checkContentToCheck;
        }

        private static StoryCreateResult ValidateCreate(IUser byUser, string url, string title, string category, string description, string userIPAddress, string userAgent)
        {
            StoryCreateResult result = null;

            if (byUser == null)
            {
                result = new StoryCreateResult { ErrorMessage = "U¿ytkownik nie mo¿e byæ pusty." };
            }
            else if (string.IsNullOrEmpty(url))
            {
                result = new StoryCreateResult { ErrorMessage = "Url nie mo¿e byæ puste." };
            }
            else if (!url.IsWebUrl())
            {
                result = new StoryCreateResult { ErrorMessage = "Niepoprawny adres url." };
            }
            else if (string.IsNullOrEmpty(title))
            {
                result = new StoryCreateResult { ErrorMessage = "Tytu³ nie mo¿e byæ pusty." };
            }
            else if (title.Trim().Length > 256)
            {
                result = new StoryCreateResult { ErrorMessage = "Tytu³ nie mo¿e zawieraæ wiêcej ni¿ 256 znaków." };
            }
            else if (string.IsNullOrEmpty(category))
            {
                result = new StoryCreateResult { ErrorMessage = "Kategoria nie mo¿e byæ pusta." };
            }
            else if (string.IsNullOrEmpty(description))
            {
                result = new StoryCreateResult { ErrorMessage = "Opis nie mo¿e byæ pusty." };
            }
            else if ((description.Trim().Length < 8) || (description.Trim().Length > 2048))
            {
                result = new StoryCreateResult { ErrorMessage = "Opis musi zawieraæ siê pomiêdzy 8 a 2048 znaków." };
            }
            else if (string.IsNullOrEmpty(userIPAddress))
            {
                result = new StoryCreateResult { ErrorMessage = "Adres Ip u¿ytkownika nie mo¿e byæ pusty." };
            }
            else if (string.IsNullOrEmpty(userAgent))
            {
                result = new StoryCreateResult { ErrorMessage = "Pole 'User agent' nie mo¿e byæ puste." };
            }

            return result;
        }

        private static CommentCreateResult ValidateComment(IStory forStory, IUser byUser, string content, string userIPAddress, string userAgent)
        {
            CommentCreateResult result = null;

            if (forStory == null)
            {
                result = new CommentCreateResult { ErrorMessage = "Wpis nie mo¿e byæ pusty." };
            }
            else if (byUser == null)
            {
                result = new CommentCreateResult { ErrorMessage = "U¿ytkownik nie mo¿e byæ pusty." };
            }
            else if (string.IsNullOrEmpty(content))
            {
                result = new CommentCreateResult { ErrorMessage = "Komentarz nie mo¿e byæ pusty." };
            }
            else if (content.Trim().Length > 2048)
            {
                result = new CommentCreateResult { ErrorMessage = "Komentarz nie mo¿e zawieraæ wiêcej ni¿ 2048 znaków." };
            }
            else if (string.IsNullOrEmpty(userIPAddress))
            {
                result = new CommentCreateResult { ErrorMessage = "Adres Ip u¿ytkownika nie mo¿ê byæ pusty." };
            }
            else if (string.IsNullOrEmpty(userAgent))
            {
                result = new CommentCreateResult { ErrorMessage = "Pole 'User agent' nie mo¿e byæ puste." };
            }

            return result;
        }

        private bool ShouldCheckSpamForUser(IUser user)
        {
            return !user.CanModerate() && (_storyRepository.CountPostedByUser(user.Id) <= _settings.StorySumittedThresholdOfUserToSpamCheck);
        }

        private T EnsureNotSpam<T>(IUser byUser, string userIPAddress, string userAgent, string url, string urlReferer, string content, string contentType, NameValueCollection serverVariables, string logMessage, string errorMessage) where T : BaseServiceResult, new()
        {
            bool isSpam = _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, content, contentType, serverVariables));

            if (isSpam)
            {
                Log.Warning(logMessage);
            }

            return isSpam ? new T { ErrorMessage = errorMessage } : null;
        }

        private string SanitizeHtml(string html)
        {
            return _htmlSanitizer.Sanitize(html);
        }

        private void AddTagsToContainers(string tags, IEnumerable<ITagContainer> tagContainers)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                string[] tagNames = tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (tagNames.Length > 0)
                {
                    tagNames = Array.ConvertAll(tagNames, t => t.NullSafe().StripHtml());
                    foreach (string tagName in tagNames.Union(tagNames))
                    {
                        if (!string.IsNullOrEmpty(tagName))
                        {
                            ITag tag = _tagRepository.FindByName(tagName);

                            if (tag == null)
                            {
                                tag = _factory.CreateTag(tagName);
                                _tagRepository.Add(tag);
                            }

                            foreach (ITagContainer container in tagContainers)
                            {
                                container.AddTag(tag);
                            }
                        }
                    }
                }
            }
        }

        private void PingStory(StoryContent content, IStory story, string detailUrl)
        {
            if (_settings.SendPing && !string.IsNullOrEmpty(content.TrackBackUrl))
            {
                _contentService.Ping(content.TrackBackUrl, story.Title, detailUrl, "Dziêkujemy za publikacjê - Trackback z {0}".FormatWith(_settings.SiteTitle), _settings.SiteTitle);
            }
        }

        private IList<PublishedStory> GetPublishableStories(DateTime currentTime)
        {
            List<PublishedStory> publishableStories = new List<PublishedStory>();

            DateTime minimumDate = currentTime.AddHours(-_settings.MaximumAgeOfStoryInHoursToPublish);
            DateTime maximumDate = currentTime.AddHours(-_settings.MinimumAgeOfStoryInHoursToPublish);

            int publishableCount = _storyRepository.CountByPublishable(minimumDate, maximumDate);

            if (publishableCount > 0)
            {
                ICollection<IStory> stories = _storyRepository.FindPublishable(minimumDate, maximumDate, 0, publishableCount).Result;

                foreach (IStory story in stories)
                {
                    PublishedStory publishedStory = new PublishedStory(story);

                    foreach (IStoryWeightCalculator strategy in _storyWeightCalculators)
                    {
                        publishedStory.Weights.Add(strategy.Name, strategy.Calculate(currentTime, story));
                    }

                    publishableStories.Add(publishedStory);
                }
            }

            return publishableStories;
        }

        private void PenaltyUsersForIncorrectlyMarkingStoriesAsSpam(IEnumerable<PublishedStory> publishableStories)
        {
            foreach (PublishedStory publishableStory in publishableStories)
            {
                ICollection<IMarkAsSpam> markedAsSpams = _markAsSpamRepository.FindAfter(publishableStory.Story.Id, publishableStory.Story.LastProcessedAt ?? publishableStory.Story.CreatedAt);

                foreach (IMarkAsSpam markedAsSpam in markedAsSpams)
                {
                    _userScoreService.StoryIncorrectlyMarkedAsSpam(markedAsSpam.ByUser);
                }
            }
        }

        private void PublishStories(DateTime currentTime, IList<PublishedStory> publishableStories)
        {
            // Now sort it based upon the score
            publishableStories = publishableStories.OrderByDescending(ps => ps.TotalScore).ToList();

            // Now assign the Rank
            publishableStories.ForEach(ps => ps.Rank = (publishableStories.IndexOf(ps) + 1));

            // Now take the stories for front page
            ICollection<PublishedStory> frontPageStories = publishableStories.OrderBy(ps => ps.Rank).Take(_settings.HtmlStoryPerPage).ToList();

            if (!frontPageStories.IsNullOrEmpty())
            {
                foreach (PublishedStory ps in frontPageStories)
                {
                    //Increase user score
                    _userScoreService.StoryPublished(ps.Story);
                    ps.Story.Publish(currentTime, ps.Rank);
                }

                //Send mail to support
                _emailSender.NotifyPublishedStories(currentTime, frontPageStories);
            }

            // Mark the Story that it has been processed
            publishableStories.ForEach(ps => ps.Story.LastProcessed(currentTime));
        }
    }
}
