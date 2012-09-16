namespace Kigg.EF.DomainObjects
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Kigg.DomainObjects;
    using Infrastructure.DomainRepositoryExtensions;
    
    public partial class Story : IStory
    {
        private const int NotSet = -1;

        private int _tagCount = NotSet;
        private int _voteCount = NotSet;
        private int _markAsSpamCount = NotSet;
        private int _viewCount = NotSet;
        private int _commentCount = NotSet;
        private int _subscriberCount = NotSet;
        
        [NonSerialized]
        private EntityCollection<ITag, Tag> _storyTags;
        [NonSerialized]
        private EntityCollection<IVote, StoryVote> _storyVotes;
        [NonSerialized]
        private EntityCollection<IMarkAsSpam, StoryMarkAsSpam> _storyMarkAsSpam;
        [NonSerialized]
        private EntityCollection<IStoryView, StoryView> _storyViews;
        [NonSerialized]
        private EntityCollection<IComment, StoryComment> _storyComments;
        [NonSerialized]
        private EntityCollection<IUser, User> _commentSubscribers;
    
        [NonSerialized]
        private EntityReference<ICategory, Category> _categoryReference;
        [NonSerialized]
        private EntityReference<IUser, User> _userReference;

        internal IEntityCollection<ITag> StoryTags
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _storyTags, StoryTagsInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_storyTags);
                return _storyTags;
            }
            set
            {
                var tags = value as EntityCollection<ITag, Tag>;
                if (tags == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<ITag, Tag>");
                }
                _storyTags = tags;
            }
        }
        internal IEntityCollection<IVote> StoryVotes
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _storyVotes, StoryVotesInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_storyVotes);
                return _storyVotes;
            }
            set
            {
                var votes = value as EntityCollection<IVote, StoryVote>;
                if (votes == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IVote, StoryVote>");
                }
                _storyVotes = votes;
            }
        }
        internal IEntityCollection<IMarkAsSpam> StoryMarkAsSpam
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _storyMarkAsSpam, StoryMarkAsSpamsInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_storyMarkAsSpam);
                return _storyMarkAsSpam;
            }
            set
            {
                var spams = value as EntityCollection<IMarkAsSpam, StoryMarkAsSpam>;
                if (spams == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IMarkAsSpam, StoryMarkAsSpam>");
                }
                _storyMarkAsSpam = spams;
            }
        }
        internal IEntityCollection<IStoryView> StoryViews
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _storyViews, StoryViewsInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_storyViews);
                return _storyViews;
            }
            set
            {
                var views = value as EntityCollection<IStoryView, StoryView>;
                if (views == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IStoryView, StoryView>");
                }
                _storyViews = views;
            }
        }
        internal IEntityCollection<IComment> StoryComments
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _storyComments, StoryCommentsInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_storyComments);
                return _storyComments;
            }
            set
            {
                var comments = value as EntityCollection<IComment, StoryComment>;
                if (comments == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IComment, StoryComment>");
                }
                _storyComments = comments;
            }
        }
        internal IEntityCollection<IUser> CommentSubscribers
        {
            get
            {
                EntityHelper.EnsureEntityCollection(ref _commentSubscribers, CommentSubscribersInternal);
                EntityHelper.EnsureEntityCollectionLoaded(_commentSubscribers);
                return _commentSubscribers;
            }
            set
            {
                var subscribers = value as EntityCollection<IUser, User>;
                if (subscribers == null)
                {
                    throw new NotSupportedException("Assigned value must be of type EntityCollection<IUser, User>");
                }
                _commentSubscribers = subscribers;
            }
        }

        public ICategory BelongsTo
        {
            [DebuggerStepThrough]
            get
            {
                if (Category == null)
                {
                    EntityHelper.EnsureEntityReference(ref _categoryReference, CategoryReference);
                    EntityHelper.EnsureEntityReferenceLoaded(_categoryReference);
                }
                return Category;
            }
        }

        public IUser PostedBy
        {
            [DebuggerStepThrough]
            get
            {
                if(User == null)
                {
                    EntityHelper.EnsureEntityReference(ref _userReference, UserReference);
                    EntityHelper.EnsureEntityReferenceLoaded(_userReference);
                }
                return User;
            }
        }

        public string FromIPAddress
        {
            [DebuggerStepThrough]
            get
            {
                return IpAddress;
            }
        }

        public ICollection<ITag> Tags
        {
            [DebuggerStepThrough]
            get
            {
                return StoryTags.OrderBy(t => t.Name).ToList().AsReadOnly();
            }
        }

        public ICollection<IVote> Votes
        {
            [DebuggerStepThrough]
            get
            {
                return StoryVotes.OrderBy(v => v.PromotedAt).ToList().AsReadOnly();
            }
        }

        public ICollection<IMarkAsSpam> MarkAsSpams
        {
            [DebuggerStepThrough]
            get
            {
                return StoryMarkAsSpam.OrderBy(s => s.MarkedAt).ToList().AsReadOnly();
            }
        }

        public ICollection<IStoryView> Views
        {
            [DebuggerStepThrough]
            get
            {
                return StoryViews.OrderBy(v => v.ViewedAt).ToList().AsReadOnly();
            }
        }

        public ICollection<IComment> Comments
        {
            [DebuggerStepThrough]
            get
            {
                return StoryComments.OrderBy(c => c.CreatedAt).ToList().AsReadOnly();
            }
        }

        public ICollection<IUser> Subscribers
        {
            [DebuggerStepThrough]
            get
            {
                return CommentSubscribers.ToList().AsReadOnly();
            }
        }

        public int TagCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_tagCount == NotSet)
                {
                    EntityHelper.EnsureEntityCollection(ref _storyTags, StoryTagsInternal);
                    var query = _storyTags.CreateSourceQuery();

                    _tagCount = query != null ? query.Count() : 0;
                }
                return _tagCount;
            }
        }

        public int VoteCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_voteCount == NotSet)
                {
                    _voteCount = this.GetVoteCount();
                }

                return _voteCount;
            }
        }

        public int MarkAsSpamCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_markAsSpamCount == NotSet)
                {
                    _markAsSpamCount = this.GetMarkAsSpamCount();
                }

                return _markAsSpamCount;
            }
        }

        public int ViewCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_viewCount == NotSet)
                {
                    _viewCount = this.GetViewCount();
                }

                return _viewCount;
            }
        }

        public int CommentCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_commentCount == NotSet)
                {
                    _commentCount = this.GetCommentCount();
                }

                return _commentCount;
            }
        }

        public int SubscriberCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_subscriberCount == NotSet)
                {
                    _subscriberCount = this.GetSubscriberCount();
                }

                return _subscriberCount;
            }
        }

        public void ChangeCategory(ICategory category)
        {
            Check.Argument.IsNotNull(category, "category");

            Category = (Category)category;
        }

        public void AddTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Id, "tag.Id");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            if (!ContainsTag(tag))
            {
                StoryTagsInternal.Add((Tag)tag);
            }
        }

        public void RemoveTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            //It should load all StoryTags then remove the desired tag
            StoryTags.Remove(StoryTags.FirstOrDefault(t => t.Name == tag.Name));
        }

        public void RemoveAllTags()
        {
            //It should load all UserTags then clear the collection
            StoryTags.Clear();
        }

        public bool ContainsTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            var tagName = tag.Name;

            EntityHelper.EnsureEntityCollection(ref _storyTags, StoryTagsInternal);
            var srcQuery = _storyTags.CreateSourceQuery();

            return StoryTagsInternal.Any(t => t.Name == tagName) || (srcQuery != null && srcQuery.Any(t => t.Name == tagName));
        }

        public void View(DateTime at, string fromIpAddress)
        {
            //Call extension method AddView, it will perform all parameters validation checks
            var view = this.AddView(at, fromIpAddress);

            //Add created view to StoryViews, this should increment views
            StoryViewsInternal.Add((StoryView)view);

            LastActivityAt = at;
        }

        public bool CanPromote(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to promote the Story if
            // 1. User has not previously promoted it.
            // 2. User has not previously marked it as spam.
            return !HasPromoted(byUser) && !HasMarkedAsSpam(byUser);
        }

        public bool Promote(DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            //Check if user can promote
            if (CanPromote(byUser))
            {
                //Call extension method AddVote, it will perform all parameters validation checks
                var vote = this.AddVote(at, byUser, fromIpAddress);

                //Add created vote to StoryVotes, this should increment votes
                StoryVotesInternal.Add((StoryVote)vote);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public bool HasPromoted(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");
            return this.GetVote(byUser) != null;
        }

        public bool CanDemote(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to demote the Story if
            // 1. Story is not posted by the same user
            // 1. User has previously promoted it.
            return !this.IsPostedBy(byUser) && HasPromoted(byUser);
        }

        public bool Demote(DateTime at, IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanDemote(byUser))
            {
                var vote = this.GetVote(byUser);
                this.RemoveVote(vote);
                StoryVotesInternal.Remove((StoryVote)vote);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public bool CanMarkAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to mark as spam when
            // 1. When Story is not published
            // 2. Story is not posted by the same user
            // 3. User has not previously promoted it.
            // 4. User has not previously marked it as spam.
            return !this.IsPublished() && 
                !this.IsPostedBy(byUser) && 
                !HasPromoted(byUser) && 
                !HasMarkedAsSpam(byUser);
        }

        public bool MarkAsSpam(DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            if (CanMarkAsSpam(byUser))
            {
                var spamStory = this.MarkSpam(at, byUser, fromIpAddress);
                StoryMarkAsSpamsInternal.Add((StoryMarkAsSpam)spamStory);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public bool HasMarkedAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            return this.GetMarkAsSpam(byUser) != null;
        }

        public bool CanUnmarkAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to unmark as spam when
            // 1. When Story is not published
            // 2. User has previously marked it as spam.
            return !this.IsPublished() && HasMarkedAsSpam(byUser);
        }

        public bool UnmarkAsSpam(DateTime at, IUser byUser)
        {
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanUnmarkAsSpam(byUser))
            {
                var spam = this.GetMarkAsSpam(byUser);
                this.UnmarkSpam(spam);
                StoryMarkAsSpamsInternal.Remove((StoryMarkAsSpam)spam);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public IComment PostComment(string content, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotEmpty(content, "content");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var comment = this.AddComment(content, at, byUser, fromIpAddress);
            StoryCommentsInternal.Add((StoryComment)comment);

            LastActivityAt = at;

            return comment;
        }

        public IComment FindComment(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return this.GetComment(id);
        }

        public void DeleteComment(IComment comment)
        {
            Check.Argument.IsNotNull(comment, "comment");

            var storyComment = (StoryComment)comment;

            this.RemoveComment(comment);

            StoryComments.Remove(storyComment);
        }

        public bool ContainsCommentSubscriber(IUser theUser)
        {
            Check.Argument.IsNotNull(theUser, "theUser");
            EntityHelper.EnsureEntityCollection(ref _commentSubscribers, CommentSubscribersInternal);
            var userName = theUser.UserName;

            var srcQuery = _commentSubscribers.CreateSourceQuery();

            return CommentSubscribersInternal.Any(u => u.UserName == userName) || (srcQuery != null && srcQuery.Any(u => u.UserName == userName));
        }

        public void SubscribeComment(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");
            //var subscribtion = this.AddCommentSubscribtion(byUser);
            
            if (ContainsCommentSubscriber(byUser)) return;

            CommentSubscribersInternal.Add((User)byUser);
        }

        public void UnsubscribeComment(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");
            if(ContainsCommentSubscriber(byUser))
            {
                CommentSubscribers.Remove(byUser);
            }
        }

        public void RemoveAllCommentSubscribers()
        {
            CommentSubscribers.Clear();
        }

        public void Approve(DateTime at)
        {
            if (!this.IsApproved())
            {
                ApprovedAt = at;
            }
        }

        public void Publish(DateTime at, int rank)
        {
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNegativeOrZero(rank, "rank");

            PublishedAt = at;
            Rank = rank;
            LastActivityAt = at;
        }

        public void LastProcessed(DateTime at)
        {
            Check.Argument.IsNotInFuture(at, "at");

            LastProcessedAt = at;
        }

        public void ChangeNameAndCreatedAt(string name, DateTime createdAt)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotInFuture(createdAt, "createdAt");

            UniqueName = name;
            CreatedAt = createdAt;
        }

        partial void OnHtmlDescriptionChanged()
        {
            TextDescription = HtmlDescription.StripHtml().Trim();
        }
        
        
    }
}
