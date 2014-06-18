using System;
using Kigg.DomainObjects;
using Kigg.Infrastructure;
using Kigg.Infrastructure.DomainRepositoryExtensions;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Repository;
    using Kigg.Test.Infrastructure;

    public class StoryExtensionFixture : BaseFixture
    {
        private readonly Mock<IStory> _story;

        public StoryExtensionFixture()
        {
            _story = new Mock<IStory>();
        }

        [Fact]
        public void IsNew_Should_Be_True_When_LastProcessedAt_Is_Null()
        {
            _story.SetupGet(s => s.LastProcessedAt).Returns((DateTime?)null);

            Assert.True(_story.Object.IsNew());
        }

        [Fact]
        public void IsPublished_Should_Be_True_When_PublishedAt_Is_Not_Null()
        {
            _story.SetupGet(s => s.PublishedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsPublished());
        }

        [Fact]
        public void HasExpired_Should_Be_True_When_CreatedAt_Is_Greater_Than_MaximumAgeToPublish()
        {
            var date = SystemTime.Now().AddHours(-(settings.Object.MaximumAgeOfStoryInHoursToPublish + 1));

            _story.SetupGet(s => s.CreatedAt).Returns(date);

            Assert.True(_story.Object.HasExpired());
        }

        [Fact]
        public void IsApproved_Should_Be_True_When_Aapproved_Is_Not_Null()
        {
            _story.SetupGet(s => s.ApprovedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsApproved());
        }

        [Fact]
        public void HasComments_Should_Be_True_When_CommentCount_Is_Greater_Than_Zero()
        {
            _story.SetupGet(s => s.CommentCount).Returns(1);

            Assert.True(_story.Object.HasComments());
        }

        [Fact]
        public void IsPostedBy_Should_Be_True_When_PostedBy_User_Id_Is_Same()
        {
            var userId = Guid.NewGuid();

            var postedByUser = new Mock<IUser>();

            postedByUser.SetupGet(u => u.Id).Returns(userId);
            _story.SetupGet(s => s.PostedBy).Returns(postedByUser.Object);

            var checkingUser = new Mock<IUser>();
            checkingUser.SetupGet(u => u.Id).Returns(userId);

            Assert.True(_story.Object.IsPostedBy(checkingUser.Object));
        }

        [Fact]
        public void Host_Returns_The_Domain_Name_From_The_Story_Url()
        {
            _story.SetupGet(s => s.Url).Returns("http://weblogs.asp.net/rashid");

            Assert.Equal("weblogs.asp.net", _story.Object.Host());
        }

        [Fact]
        public void SmallThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Setup(t => t.For(It.IsAny<string>(), ThumbnailSize.Small)).Returns("http://thumbnail.com").Verifiable();

            _story.SetupGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.SmallThumbnail();

            thumbnail.Verify();
        }

        [Fact]
        public void MediumThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Setup(t => t.For(It.IsAny<string>(), ThumbnailSize.Medium)).Returns("http://thumbnail.com").Verifiable();

            _story.SetupGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.MediumThumbnail();

            thumbnail.Verify();
        }

        [Fact]
        public void GetViewCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IStoryViewRepository>(10);
            int i = _story.Object.GetViewCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetVoteCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IVoteRepository>(10);
            int i = _story.Object.GetVoteCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetMarkAsSpamCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IMarkAsSpamRepository>(10);
            int i = _story.Object.GetMarkAsSpamCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetCommentCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<ICommentRepository>(10);
            int i = _story.Object.GetCommentCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetSubscriberCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<ICommentSubscribtionRepository>(10);
            int i = _story.Object.GetSubscriberCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void AddView_Should_Use_IDomainObjectFactory_And_IStoryViewRepository()
        {
            var repository = SetupResolve<IStoryViewRepository>();
            var domFactory = SetupResolve<IDomainObjectFactory>();
            
            var now = SystemTime.Now();

            domFactory.Setup(f => f.CreateStoryView(_story.Object, now, "127.0.0.1")).Returns(
                It.IsAny<IStoryView>()).Verifiable();
            
            _story.Object.AddView(now, "127.0.0.1");

            domFactory.Verify();
            repository.Verify(r => r.Add(It.IsAny<IStoryView>()),Times.AtMostOnce());
            
        }
        
        [Fact]
        public void AddVote_Should_Use_IDomainObjectFactory_And_IVoteRepository()
        {
            var repository = SetupResolve<IVoteRepository>();
            var domFactory = SetupResolve<IDomainObjectFactory>();

            var now = SystemTime.Now();

            domFactory.Setup(f => f.CreateStoryVote(_story.Object, now, It.IsAny<IUser>(), "127.0.0.1")).Returns(
                It.IsAny<IVote>()).Verifiable();

            _story.Object.AddVote(now, new Mock<IUser>().Object, "127.0.0.1");

            domFactory.Verify();
            repository.Verify(r => r.Add(It.IsAny<IVote>()),Times.AtMostOnce());
        }
        
        [Fact]
        public void RemoveVote_Should_Use_IVoteRepository()
        {
            var repository = SetupResolve<IVoteRepository>();

            var vote = new Mock<IVote>().Object;

            _story.Object.RemoveVote(vote);

            repository.Verify(r => r.Remove(vote),Times.AtMostOnce());
        }

        [Fact]
        public void GetVote_Should_Use_IVoteRepository()
        {
            var repository = SetupResolve<IVoteRepository>();

            var user = new Mock<IUser>().Object;
            
            _story.Object.GetVote(user);

            repository.Verify(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>()),Times.AtMostOnce());
        }

        [Fact]
        public void MarkSpam_Should_Use_IDomainObjectFactory_And_IMarkAsSpamRepository()
        {
            var repository = SetupResolve<IMarkAsSpamRepository>();
            var domFactory = SetupResolve<IDomainObjectFactory>();

            var now = SystemTime.Now();

            domFactory.Setup(f => f.CreateMarkAsSpam(_story.Object, now, It.IsAny<IUser>(), "127.0.0.1")).Returns(It.IsAny<IMarkAsSpam>()).Verifiable();

            _story.Object.MarkSpam(now, new Mock<IUser>().Object, "127.0.0.1");

            domFactory.Verify();
            repository.Verify(r => r.Add(It.IsAny<IMarkAsSpam>()),Times.AtMostOnce());
        }

        [Fact]
        public void GetMarkAsSpam_Should_Use_IMarkAsSpamRepository()
        {
            var repository = SetupResolve<IMarkAsSpamRepository>();

            var user = new Mock<IUser>().Object;

            _story.Object.GetMarkAsSpam(user);

            repository.Verify(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>()),Times.AtMostOnce());
        }

        [Fact]
        public void UnmarkSpam_Should_Use_IMarkAsSpamRepository()
        {
            var repository = SetupResolve<IMarkAsSpamRepository>();

            var spam = new Mock<IMarkAsSpam>().Object;
            
            _story.Object.UnmarkSpam(spam);

            repository.Verify(r => r.Remove(spam),Times.AtMostOnce());
        }

        [Fact]
        public void AddComment_Should_Use_IDomainObjectFactory_And_ICommentRepository()
        {
            var repository = SetupResolve<ICommentRepository>();
            var domFactory = SetupResolve<IDomainObjectFactory>();

            var now = SystemTime.Now();

            domFactory.Setup(f => f.CreateComment(_story.Object, "dummy content", now, It.IsAny<IUser>(), "127.0.0.1")).Returns(
                It.IsAny<IComment>()).Verifiable();

            _story.Object.AddComment("dummy content",now, new Mock<IUser>().Object, "127.0.0.1");

            domFactory.Verify();
            repository.Verify(r => r.Add(It.IsAny<IComment>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetComment_Should_Use_ICommentRepository()
        {
            var repository = SetupResolve<ICommentRepository>();

            _story.Object.FindComment(It.IsAny<Guid>());

            repository.Verify(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.AtMostOnce());
        }

        [Fact]
        public void DeleteComment_Should_Use_ICommentRepository()
        {
            var repository = SetupResolve<ICommentRepository>();

            var comment = new Mock<IComment>().Object;

            _story.Object.DeleteComment(comment);

            repository.Verify(r => r.Remove(comment), Times.AtMostOnce());
        }

        [Fact]
        public void GetCommentSubscribtion_Should_Use_ICommentSubscribtionRepository()
        {
            var repository = SetupResolve<ICommentSubscribtionRepository>();

            var user = new Mock<IUser>().Object;

            _story.Object.GetCommentSubscribtion(user);

            repository.Verify(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.AtMostOnce());
        }

        [Fact]
        public void AddCommentSubscribtion_Should_Use_ICommentSubscribtionRepository()
        {
            var repository = SetupResolve<ICommentSubscribtionRepository>();
            
            repository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(null as ICommentSubscribtion);

            repository.Verify(r => r.Add(It.IsAny<ICommentSubscribtion>()), Times.AtMostOnce());
        }

        [Fact]
        public void AddCommentSubscribtion_Should_Never_Call_ICommentSubscribtionRepository_Add()
        {
            var repository = SetupResolve<ICommentSubscribtionRepository>();

            repository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Mock<ICommentSubscribtion>().Object);

            repository.Verify(r => r.Add(It.IsAny<ICommentSubscribtion>()), Times.Never());
        }

        [Fact]
        public void RemoveCommentSubscribtion_Should_Use_ICommentSubscribtionRepository()
        {
            var repository = SetupResolve<ICommentSubscribtionRepository>();
            
            repository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(
                new Mock<ICommentSubscribtion>().Object);

            repository.Verify(r => r.Remove(It.IsAny<ICommentSubscribtion>()), Times.AtMostOnce());
        }

        [Fact]
        public void RemoveCommentSubscribtion_Should_Never_Call_ICommentSubscribtionRepository_Remove()
        {
            var repository = SetupResolve<ICommentSubscribtionRepository>();

            repository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(null as ICommentSubscribtion);

            repository.Verify(r => r.Remove(It.IsAny<ICommentSubscribtion>()), Times.Never());
        }

        private void SetupCountByStoryRepository<T>(int count) where T : class, ICountByStoryRepository
        {
            var repository = SetupResolve<T>();
            repository.Setup(r => r.CountByStory(_story.Object.Id)).Returns(count);
        }

        
    }
}
