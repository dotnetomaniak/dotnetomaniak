using System;
using System.Collections.Generic;
using System.Linq;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using DomainObjects;
    using Kigg.EF.DomainObjects;

    public class StoryFixture : EfBaseFixture
    {
        private readonly Story _story;
        private readonly Mock<EntityCollection<ITag, Tag>> _mockedTags;
        private readonly Mock<EntityCollection<IVote, StoryVote>> _mockedVotes;
        private readonly Mock<EntityCollection<IMarkAsSpam, StoryMarkAsSpam>> _mockedMarkAsSpam;
        private readonly Mock<EntityCollection<IStoryView, StoryView>> _mockedViews;
        private readonly Mock<EntityCollection<IComment, StoryComment>> _mockedComments;
        private readonly Mock<EntityCollection<IUser, User>> _mockedUsers;
        
        public StoryFixture()
        {
            _story = new Story();
            
            _mockedTags = new Mock<EntityCollection<ITag, Tag>>(_story.StoryTagsInternal);
            _mockedVotes = new Mock<EntityCollection<IVote, StoryVote>>(_story.StoryVotesInternal);
            _mockedMarkAsSpam = new Mock<EntityCollection<IMarkAsSpam, StoryMarkAsSpam>>(_story.StoryMarkAsSpamsInternal);
            _mockedViews = new Mock<EntityCollection<IStoryView, StoryView>>(_story.StoryViewsInternal);
            _mockedComments = new Mock<EntityCollection<IComment, StoryComment>>(_story.StoryCommentsInternal);
            _mockedUsers = new Mock<EntityCollection<IUser, User>>(_story.CommentSubscribersInternal);

            _story.StoryTags = _mockedTags.Object;
            _story.StoryVotes = _mockedVotes.Object;
            _story.StoryMarkAsSpam = _mockedMarkAsSpam.Object;
            _story.StoryViews = _mockedViews.Object;
            _story.StoryComments = _mockedComments.Object;
            _story.CommentSubscribers = _mockedUsers.Object;
        }

        [Fact]
        public void BelongsTo_Should_Return_The_Category()
        {
            _story.Category = new Category();

            Assert.Same(_story.BelongsTo, _story.Category);
        }

        [Fact]
        public void PostedBy_Should_Return_The_User()
        {
            _story.User = new User();

            Assert.Same(_story.PostedBy, _story.User);
        }

        [Fact]
        public void FromIPAddress_Should_Return_IPAddress()
        {
            _story.IpAddress = "192.168.0.1";

            Assert.Equal(_story.FromIPAddress, _story.IpAddress);
        }

        [Fact]
        public void Tags_Should_Call_EntityCollection_Load_When_IsLoaded_False()
        {
            _mockedTags.SetupGet(t => t.IsLoaded).Returns(false);
            Assert.NotNull(_story.Tags);
            _mockedTags.Verify(t => t.Load(), Times.AtMostOnce());
        }

        [Fact]
        public void Tags_Should_Not_Call_EntityCollection_Load_When_IsLoaded_True()
        {
            _mockedTags.SetupGet(t => t.IsLoaded).Returns(true);
            Assert.NotNull(_story.Tags);
            _mockedTags.Verify(t => t.Load(), Times.Never());
        }

        [Fact]
        public void Tags_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.Tags);
        }

        [Fact]
        public void Votes_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.Votes);
        }

        [Fact]
        public void MarkAsSpams_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.MarkAsSpams);
        }

        [Fact]
        public void Views_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.Views);
        }

        [Fact]
        public void Comments_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.Comments);
        }

        [Fact]
        public void Subscribers_Should_Be_Empty_When_New_Instance_Is_Created()
        {
            Assert.Empty(_story.Subscribers);
        }

        [Fact]
        public void TagCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            Assert.True(_story.TagCount == 0);
        }

        [Fact]
        public void VoteCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var votes = new List<StoryVote>();

            database.SetupGet(db => db.VoteDataSource).Returns(votes.AsQueryable());

            Assert.True(_story.VoteCount == 0);
        }

        [Fact]
        public void VoteCount_Should_Use_VoteRepository()
        {
            voteRepository.Setup(r => r.CountByStory(It.IsAny<Guid>())).Returns(0).Verifiable();

            #pragma warning disable 168
            var count = _story.VoteCount;
            #pragma warning restore 168

            voteRepository.Verify();
        }

        [Fact]
        public void MarkAsSpamCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var markAsSpams = new List<StoryMarkAsSpam>();

            database.SetupGet(db => db.MarkAsSpamDataSource).Returns(markAsSpams.AsQueryable());

            Assert.True(_story.MarkAsSpamCount == 0);
        }

        [Fact]
        public void MarkAsSpamCount_Should_Use_MarkAsSpamaRepository()
        {
            markAsSpamRepository.Setup(r => r.CountByStory(It.IsAny<Guid>())).Returns(0).Verifiable();

            #pragma warning disable 168
            var count = _story.MarkAsSpamCount;
            #pragma warning restore 168

            markAsSpamRepository.Verify();
        }

        [Fact]
        public void ViewCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var storyViews = new List<StoryView>();

            database.SetupGet(db => db.StoryViewDataSource).Returns(storyViews.AsQueryable());

            Assert.True(_story.MarkAsSpamCount == 0);
        }

        [Fact]
        public void ViewCount_Should_Use_StoryViewRepository()
        {
            storyViewRepository.Setup(r => r.CountByStory(It.IsAny<Guid>())).Returns(0).Verifiable();

            #pragma warning disable 168
            var count = _story.ViewCount;
            #pragma warning restore 168

            storyViewRepository.Verify();
        }

        [Fact]
        public void CommentCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            var comments = new List<StoryComment>();

            database.SetupGet(db => db.CommentDataSource).Returns(comments.AsQueryable());

            Assert.True(_story.CommentCount == 0);
        }

        [Fact]
        public void CommentCount_Should_Use_CommentRepository()
        {
            commentRepository.Setup(r => r.CountByStory(It.IsAny<Guid>())).Returns(0).Verifiable();

            #pragma warning disable 168
            var count = _story.CommentCount;
            #pragma warning restore 168

            commentRepository.Verify();
        }

        [Fact]
        public void SubscriberCount_Should_Be_Zero_When_New_Instance_Is_Created()
        {
            Assert.True(_story.SubscriberCount == 0);
        }

        [Fact]
        public void ChangeCategory_Should_Update_BelongsTo()
        {
            var category = new Category();

            _story.ChangeCategory(category);

            Assert.Same(_story.BelongsTo, category);
        }

        [Fact]
        public void AddTag_Should_Increase_Tags_Collection()
        {
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, _story.StoryTagsInternal.Count);
            Assert.Equal(1, _story.Tags.Count);
        }

        [Fact]
        public void RemoveTag_Should_Decrease_Tags_Collection()
        {
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.Equal(1, _story.StoryTagsInternal.Count);
            Assert.Equal(1, _story.Tags.Count);

            _story.RemoveTag(new Tag { Name = "Dummy" });

            Assert.Equal(0, _story.StoryTagsInternal.Count);
            Assert.Equal(0, _story.Tags.Count);
        }

        [Fact]
        public void RemoveAllTag_Should_Clear_Tags_Collection()
        {
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy1" });
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy2" });
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy3" });

            Assert.Equal(3, _story.StoryTagsInternal.Count);
            Assert.Equal(3, _story.Tags.Count);

            _story.RemoveAllTags();

            Assert.Empty(_story.StoryTagsInternal);
            Assert.Empty(_story.Tags);
        }

        [Fact]
        public void ContainsTag_Should_Return_True_When_Tag_Exists_In_Tags_Collection()
        {
            _story.AddTag(new Tag { Id = Guid.NewGuid(), Name = "Dummy" });

            Assert.True(_story.ContainsTag(new Tag { Name = "Dummy" }));
        }

        [Fact]
        public void View_Should_Increase_Views_Collection()
        {
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";
            domainObjectFactory.Setup(f => f.CreateStoryView(_story, at, fromIpAddress)).Returns(new StoryView());

            _story.View(at, fromIpAddress);

            Assert.Equal(1, _story.StoryViewsInternal.Count);
        }

        [Fact]
        public void View_Should_Update_LastActivityAt()
        {
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";
            domainObjectFactory.Setup(f => f.CreateStoryView(_story, at, fromIpAddress)).Returns(new StoryView());

            _story.View(at, fromIpAddress);

            Assert.Equal(at, _story.LastActivityAt);
        }

        [Fact]
        public void View_Should_Use_DomainObjectFactory_And_StoryViewRepository()
        {
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(f => f.CreateStoryView(_story, at, fromIpAddress)).Returns(new StoryView()).Verifiable();

            _story.View(at, fromIpAddress);
            domainObjectFactory.Verify();
            storyViewRepository.Verify(r => r.Add(It.IsAny<IStoryView>()), Times.AtMostOnce());
        }

        [Fact]
        public void CanPromote_Should_Return_True_When_Story_Has_Not_Been_Promoted_By_The_User()
        {
            Assert.True(_story.CanPromote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanPromote_Should_Return_True_When_Story_Has_Not_Been_MarkedAsSpam_By_The_User()
        {
            Assert.True(_story.CanPromote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanPromote_Should_Return_False_When_Story_Has_Been_Promoted_By_User()
        {
            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote());

            Assert.False(_story.CanPromote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanPromote_Should_Return_False_When_Story_Has_Been_Marked_As_Spam_By_The_User()
        {
            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryMarkAsSpam());

            Assert.False(_story.CanPromote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void Promote_Should_Return_True_When_User_Can_Promote()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote());

            Assert.True(_story.Promote(at, user, fromIpAddress));
        }

        [Fact]
        public void Promote_Should_Return_False_When_User_Can_Not_Promote()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote());

            Assert.False(_story.Promote(at, user, fromIpAddress));

            domainObjectFactory.Verify(r => r.CreateStoryVote(_story, at, user, fromIpAddress), Times.Never());
            voteRepository.Verify(r => r.Add(It.IsAny<StoryVote>()), Times.Never());
        }

        [Fact]
        public void Promote_Should_Use_DomainObjectFactory_And_VoteRepository()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote()).Verifiable();

            _story.Promote(at, user, fromIpAddress);

            domainObjectFactory.Verify();
            voteRepository.Verify(r => r.Add(It.IsAny<StoryVote>()), Times.AtMostOnce());
        }

        [Fact]
        public void Promote_Should_Increase_Votes_Collection()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote());

            _story.Promote(at, user, fromIpAddress);

            Assert.Equal(1, _story.StoryVotesInternal.Count);
            Assert.Equal(1, _story.Votes.Count);
        }

        [Fact]
        public void Promote_Should_Update_Last_Activity_At()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote());

            _story.Promote(at, user, fromIpAddress);

            Assert.Equal(at, _story.LastActivityAt);
        }

        [Fact]
        public void HasPromoted_Should_Return_False_When_User_Has_Not_Promoted_The_Story()
        {
            Assert.False(_story.HasPromoted(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void HasPromoted_Should_Use_VoteRepository()
        {
            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((IVote)null).Verifiable();

            _story.HasPromoted(new User { Id = Guid.NewGuid() });

            voteRepository.Verify();
        }

        [Fact]
        public void CanDemote_Should_Return_False_When_User_Has_Not_Promoted_The_Story()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            Assert.False(_story.CanDemote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanDemote_Should_Return_False_When_Story_Is_Posted_By_The_Same_User()
        {
            var user = new User { Id = Guid.NewGuid() };

            _story.User = user;

            Assert.False(_story.CanDemote(user));
        }

        [Fact]
        public void CanDemote_Should_Return_True_When_User_Has_Previously_Promoted_The_Story()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote());

            Assert.True(_story.CanDemote(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void Demote_Should_Return_True_When_User_Can_Demote()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote());

            Assert.True(_story.Demote(SystemTime.Now(), new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void Demote_Should_Return_False_When_User_Can_Not_Demote()
        {
            var user = new User { Id = Guid.NewGuid() };

            _story.User = user;

            Assert.False(_story.Demote(SystemTime.Now(), user));

            voteRepository.Verify(r => r.Remove(It.IsAny<StoryVote>()), Times.Never());
        }

        [Fact]
        public void Demote_Should_Use_VoteRepository()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote()).Verifiable();

            _story.Demote(SystemTime.Now(), new User { Id = Guid.NewGuid() });

            voteRepository.Verify(r => r.Remove(It.IsAny<StoryVote>()), Times.AtMostOnce());
            voteRepository.Verify();
        }

        [Fact]
        public void Demote_Should_Decrease_Votes_Collection()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";
            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote());

            _story.Promote(at, user, fromIpAddress);

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(_story.Votes.ElementAt(0));
            _story.Demote(SystemTime.Now(), user);

            Assert.Equal(0, _story.StoryVotesInternal.Count);
            Assert.Equal(0, _story.Votes.Count);
        }

        [Fact]
        public void Demote_Should_Update_Last_Activity_At()
        {
            var user = new User { Id = Guid.NewGuid() };
            var at = SystemTime.Now().AddDays(-5);
            const string fromIpAddress = "192.168.0.1";
            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(r => r.CreateStoryVote(_story, at, user, fromIpAddress)).Returns(new StoryVote());

            _story.Promote(at, user, fromIpAddress);

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(_story.Votes.ElementAt(0));

            DateTime lastValue = _story.LastActivityAt;

            _story.Demote(SystemTime.Now(), user);

            Assert.NotEqual(_story.LastActivityAt, lastValue);
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_False_When_Story_Is_Published()
        {
            _story.PublishedAt = SystemTime.Now().AddMonths(-1);

            Assert.False(_story.CanMarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_False_When_Story_Is_Posted_By_The_Same_User()
        {
            var user = new User { Id = Guid.NewGuid() };

            _story.User = user;

            Assert.False(_story.CanMarkAsSpam(user));
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_False_When_Story_Is_Promoted_By_The_Same_User()
        {
            var user = new User { Id = Guid.NewGuid() };

            _story.User = user;

            voteRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryVote());

            Assert.False(_story.CanMarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_False_When_Story_Is_Already_Marked_As_Spam_By_The_Same_User()
        {
            var user = new User { Id = Guid.NewGuid() };

            _story.User = user;

            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryMarkAsSpam());

            Assert.False(_story.CanMarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_True_When_Story_Has_Not_Been_Promoted_By_The_User()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            Assert.True(_story.CanMarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanMarkAsSpam_Should_Return_True_When_Story_Has_Not_Been_MarkedAsSpam_By_The_User()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            Assert.True(_story.CanMarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void MarkAsSpam_Should_Return_True_When_User_Can_Mark_As_Spam()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(r => r.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(new StoryMarkAsSpam());

            Assert.True(_story.MarkAsSpam(at, user, "192.168.0.1"));
        }

        [Fact]
        public void MarkAsSpam_Should_Return_False_When_User_Can_Not_Mark_As_Spam()
        {
            _story.PublishedAt = SystemTime.Now().AddDays(-1);

            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            Assert.False(_story.MarkAsSpam(at, user, fromIpAddress));

            domainObjectFactory.Verify(r => r.CreateMarkAsSpam(_story, at, user, fromIpAddress), Times.Never());
        }

        [Fact]
        public void MarkAsSpam_Should_Use_DomainObjectFactory_And_MarkAsSpamRepository()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(f => f.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(
                new StoryMarkAsSpam()).Verifiable();

            _story.MarkAsSpam(at, user, fromIpAddress);

            domainObjectFactory.Verify();
            markAsSpamRepository.Verify(r => r.Add(It.IsAny<IMarkAsSpam>()), Times.AtMostOnce());
        }

        [Fact]
        public void MarkAsSpam_Should_Increase_MarkAsSpams_Collection()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(f => f.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(
                new StoryMarkAsSpam());

            _story.MarkAsSpam(at, user, fromIpAddress);

            Assert.Equal(1, _story.StoryMarkAsSpamsInternal.Count);
            Assert.Equal(1, _story.MarkAsSpams.Count);
        }

        [Fact]
        public void MarkAsSpam_Should_Update_Last_Activity_At()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            var lastValue = _story.LastActivityAt;

            domainObjectFactory.Setup(f => f.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(
                new StoryMarkAsSpam());

            _story.MarkAsSpam(at, user, fromIpAddress);

            Assert.NotEqual(lastValue, _story.LastActivityAt);
        }

        [Fact]
        public void HasMarkedAsSpam_Should_Return_False_When_User_Has_Not_Marked_The_Story_As_Spam()
        {
            Assert.False(_story.HasMarkedAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void HasMarkedAsSpam_Should_Use_MarkAsSpamRepository()
        {
            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((IMarkAsSpam)null).Verifiable();

            _story.HasMarkedAsSpam(new User { Id = Guid.NewGuid() });

            markAsSpamRepository.Verify();
        }

        [Fact]
        public void CanUnmarkAsSpam_Should_Return_False_When_User_Has_Not_Marked_The_Story_As_Spam()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            Assert.False(_story.CanUnmarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void CanUnmarkAsSpam_Should_Return_False_When_Story_Is_Published()
        {
            _story.PublishedAt = SystemTime.Now().AddDays(-1);

            Assert.False(_story.CanUnmarkAsSpam(new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void UnmarkAsSpam_Should_Return_True_When_User_Can_Unmark_As_Spam()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryMarkAsSpam());

            Assert.True(_story.UnmarkAsSpam(SystemTime.Now(), new User { Id = Guid.NewGuid() }));
        }

        [Fact]
        public void UnmarkAsSpam_Should_Return_False_When_User_Can_Not_Unmark_As_Spam()
        {
            _story.PublishedAt = SystemTime.Now().AddDays(-1);

            Assert.False(_story.UnmarkAsSpam(SystemTime.Now(), new User { Id = Guid.NewGuid() }));

            markAsSpamRepository.Verify(r => r.Add(It.IsAny<IMarkAsSpam>()), Times.AtMostOnce());
        }

        [Fact]
        public void UnmarkAsSpam_Should_Use_MarkAsSpamRepository()
        {
            _story.User = new User { Id = Guid.NewGuid() };

            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new StoryMarkAsSpam());

            _story.UnmarkAsSpam(SystemTime.Now(), new User { Id = Guid.NewGuid() });

            markAsSpamRepository.Verify(r => r.Remove(It.IsAny<StoryMarkAsSpam>()), Times.AtMostOnce());
        }

        [Fact]
        public void UnmarkAsSpam_Should_Decrease_MarkAsSpams_Collection()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(f => f.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(new StoryMarkAsSpam());

            _story.MarkAsSpam(at, user, fromIpAddress);

            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(_story.StoryMarkAsSpamsInternal.ElementAt(0));

            _story.UnmarkAsSpam(SystemTime.Now(), user);

            Assert.Equal(0, _story.StoryMarkAsSpamsInternal.Count);
            Assert.Equal(0, _story.MarkAsSpams.Count);
        }

        [Fact]
        public void UnmarkAsSpam_Should_Update_Last_Activity_At()
        {
            var at = SystemTime.Now();
            var user = new User { Id = Guid.NewGuid() };
            const string fromIpAddress = "192.168.0.1";

            _story.User = new User { Id = Guid.NewGuid() };

            domainObjectFactory.Setup(f => f.CreateMarkAsSpam(_story, at, user, fromIpAddress)).Returns(new StoryMarkAsSpam());

            _story.MarkAsSpam(at, user, fromIpAddress);

            markAsSpamRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(_story.MarkAsSpams.ElementAt(0));

            DateTime lastValue = _story.LastActivityAt;

            _story.UnmarkAsSpam(SystemTime.Now(), user);

            Assert.NotEqual(_story.LastActivityAt, lastValue);
        }

        [Fact]
        public void PostComment_Should_Return_New_Comment()
        {
            var comment = SetupAndPostComment();

            Assert.NotNull(comment);
        }

        [Fact]
        public void PostComment_Should_Use_CommentRepository()
        {
            var comment = SetupAndPostComment();

            commentRepository.Verify(r => r.Add(comment), Times.AtMostOnce());
        }

        [Fact]
        public void PostComment_Should_Increase_Comments_Collection()
        {
            SetupAndPostComment();

            Assert.Equal(1, _story.StoryCommentsInternal.Count);
            Assert.Equal(1, _story.Comments.Count);
        }

        [Fact]
        public void PostComment_Should_Update_LastActivityAt()
        {
            var lastValue = _story.LastActivityAt;

            SetupAndPostComment();

            Assert.NotEqual(lastValue, _story.LastActivityAt);
        }

        [Fact]
        public void FindComment_Should_Return_Null_When_Comment_Does_Not_Exist_In_Comments_Collection()
        {
            Assert.Null(_story.FindComment(Guid.NewGuid()));
        }

        [Fact]
        public void FindComment_Should_Use_CommentRepository()
        {
            commentRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((IComment)null).Verifiable();

            #pragma warning disable 168
            var comment = _story.FindComment(Guid.NewGuid());
            #pragma warning restore 168

            commentRepository.Verify();
        }

        [Fact]
        public void DeleteComment_Should_Use_CommentRepository()
        {
            var comment = SetupAndPostComment();

            _story.DeleteComment(comment);

            commentRepository.Verify(r => r.Remove(It.IsAny<IComment>()), Times.AtMostOnce());
        }

        [Fact]
        public void DeleteComment_Should_Decrease_Comment_Collection()
        {
            var comment = SetupAndPostComment();

            _story.DeleteComment(comment);

            Assert.Equal(0, _story.StoryCommentsInternal.Count);
            Assert.Equal(0, _story.Comments.Count);
        }

        [Fact]
        public void ContainsCommentSubscriber_Should_Return_False_When_Subscriber_Does_Not_Exist_In_Subscribers_Collection()
        {
            Assert.False(_story.ContainsCommentSubscriber(new User { Id = Guid.NewGuid() }));
        }

        //[Fact]
        //public void ContainsCommentSubscriber_Should_Use_CommentSubscribtionRepository()
        //{
        //    commentSubscribtionRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((ICommentSubscribtion)null).Verifiable();

        //    #pragma warning disable 168
        //    var exists = _story.ContainsCommentSubscriber(new User { Id = Guid.NewGuid() });
        //    #pragma warning restore 168

        //    commentSubscribtionRepository.Verify();
        //}
        //[Fact]
        //public void SubscribeComment_Should_Use_CommentSubscribtionRepository()
        //{
        //    var user = new User { Id = Guid.NewGuid() };
        //    SetupAndSubscribeComment(user);
        //    commentSubscribtionRepository.Verify(r => r.Add(It.IsAny<ICommentSubscribtion>()), Times.AtMostOnce());
        //}

        [Fact]
        public void SubscribeComment_Should_Increase_Subscribers_Collection()
        {
            var user = new User { Id = Guid.NewGuid() };
            _story.SubscribeComment(user);
            Assert.Equal(1, _story.Subscribers.Count);
        }

        //[Fact]
        //public void UnsubscribeComment_Should_Use_CommentSubscribtionRepository()
        //{
        //    var user = new User { Id = Guid.NewGuid() };
        //    SetupAndSubscribeComment(user);

        //    commentSubscribtionRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(_story.CommentSubscribtions[0]).Verifiable();
        //    commentSubscribtionRepository.Setup(r => r.Remove(It.IsAny<ICommentSubscribtion>())).Verifiable();

        //    _story.UnsubscribeComment(user);

        //    commentSubscribtionRepository.Verify();
        //}

        [Fact]
        public void UnsubscribeComment_Should_Decrease_Subscribers()
        {
            var user = new User { Id = Guid.NewGuid() };
            _story.SubscribeComment(user);

            var subscribtion = new CommentSubscribtion(_story, user);
            commentSubscribtionRepository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(subscribtion);

            _story.UnsubscribeComment(user);

            Assert.Equal(0, _story.CommentSubscribersInternal.Count);
            Assert.Equal(0, _story.Subscribers.Count);
        }

        [Fact]
        public void Approve_Should_Update_ApprovedAt()
        {
            var at = SystemTime.Now();

            _story.Approve(at);

            Assert.Equal(at, _story.ApprovedAt);
        }

        [Fact]
        public void Publish_Should_Update_PublishedAt()
        {
            var at = SystemTime.Now();

            _story.Publish(at, 1);

            Assert.Equal(at, _story.PublishedAt);
        }

        [Fact]
        public void Publish_Should_Update_Rank()
        {
            _story.Publish(SystemTime.Now(), 1);

            Assert.Equal(1, _story.Rank);
        }

        [Fact]
        public void LastProcessed_Should_Update_LastProcessedAt()
        {
            var at = SystemTime.Now();

            _story.LastProcessed(at);

            Assert.Equal(at, _story.LastProcessedAt);
        }

        [Fact]
        public void ChangeNameAndCreatedAt_Should_Update_UniqueName()
        {
            _story.ChangeNameAndCreatedAt("xxx", SystemTime.Now());

            Assert.Equal("xxx", _story.UniqueName);
        }

        [Fact]
        public void ChangeNameAndCreatedAt_Should_Update_CreatedAt()
        {
            var at = SystemTime.Now();

            _story.ChangeNameAndCreatedAt("xxx", at);

            Assert.Equal(at, _story.CreatedAt);
        }

        private IComment SetupAndPostComment()
        {
            var user = new User { Id = Guid.NewGuid() };
            const string content = "This is a comment";
            var at = SystemTime.Now();
            const string fromIpAddress = "192.168.0.1";

            domainObjectFactory.Setup(f => f.CreateComment(_story, content, at, user, fromIpAddress)).Returns(
                new StoryComment());

            var comment = _story.PostComment(content, at, user, fromIpAddress);
            return comment;
        }

        
    }
}
