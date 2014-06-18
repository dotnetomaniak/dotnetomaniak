using System;
using System.Collections.Generic;
using Kigg.DomainObjects;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class LoggingStoryRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<IStoryRepository> _innerRepository;
        private readonly LoggingStoryRepository _loggingRepository;

        public LoggingStoryRepositoryFixture()
        {
            _innerRepository = new Mock<IStoryRepository>();
            _loggingRepository = new LoggingStoryRepository(_innerRepository.Object);
        }

        [Fact]
        public void Add_Should_Log_Info()
        {
            Add();

            log.Verify();
        }

        [Fact]
        public void Add_Should_Use_InnerRepository()
        {
            Add();

            _innerRepository.Verify();
        }

        [Fact]
        public void Remove_Should_Log_Warning()
        {
            Remove();

            log.Verify();
        }

        [Fact]
        public void Remove_Should_Use_InnerRepository()
        {
            Remove();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Info_When_Story_Exists()
        {
            FindById(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindById(null);

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Use_InnerRepository()
        {
            FindById(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Info_When_Story_Exists()
        {
            FindByUniqueName(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindByUniqueName(null);

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Use_InnerRepository()
        {
            FindByUniqueName(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository_And_Log_When_Story_Exists()
        {
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository_And_Log_When_Story_Does_Not_Exist()
        {
            _innerRepository.Setup(r => r.FindByUrl(It.IsAny<String>())).Returns((IStory) null).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.FindByUrl("http://story.com");
        }

        [Fact]
        public void FindByUrl_Should_Log_Info_When_Story_Exists()
        {
            FindByUrl(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindByUrl(null);

            log.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository()
        {
            FindByUrl(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublished_Should_Log_Info_When_Stories_Exist()
        {
            FindPublished(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublished_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublished(null);

            log.Verify();
        }

        [Fact]
        public void FindPublished_Should_Use_InnerRepository()
        {
            FindPublished(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Log_Info_When_Stories_Exist()
        {
            FindPublishedByCategory(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublishedByCategory(null);

            log.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Use_InnerRepository()
        {
            FindPublishedByCategory(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Log_Info_When_Stories_Exist()
        {
            FindUpcoming(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindUpcoming(null);

            log.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Use_InnerRepository()
        {
            FindUpcoming(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindNew_Should_Log_Info_When_Stories_Exist()
        {
            FindNew(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindNew_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindNew(null);

            log.Verify();
        }

        [Fact]
        public void FindNew_Should_Use_InnerRepository()
        {
            FindNew(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindUnapproved_Should_Log_Info_When_Stories_Exist()
        {
            FindUnapproved(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindUnapproved_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindUnapproved(null);

            log.Verify();
        }

        [Fact]
        public void FindUnapproved_Should_Use_InnerRepository()
        {
            FindUnapproved(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Log_Info_When_Stories_Exist()
        {
            FindPublishable(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublishable(null);

            log.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Use_InnerRepository()
        {
            FindPublishable(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByTag_Should_Log_Info_When_Stories_Exist()
        {
            FindByTag(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindByTag_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindByTag(null);

            log.Verify();
        }

        [Fact]
        public void FindByTag_Should_Use_InnerRepository()
        {
            FindByTag(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void Search_Should_Log_Info_When_Stories_Exist()
        {
            Search(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void Search_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            Search(null);

            log.Verify();
        }

        [Fact]
        public void Search_Should_Use_InnerRepository()
        {
            Search(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindPostedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPostedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Use_InnerRepository()
        {
            FindPostedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindPromotedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPromotedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Use_InnerRepository()
        {
            FindPromotedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindCommentedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindCommentedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Use_InnerRepository()
        {
            FindCommentedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByPublished_Should_Log_Info()
        {
            CountByPublished();

            log.Verify();
        }

        [Fact]
        public void CountByPublished_Should_Use_InnerRepository()
        {
            CountByPublished();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByUpcoming_Should_Log_Info()
        {
            CountByUpcoming();

            log.Verify();
        }

        [Fact]
        public void CountByUpcoming_Should_Use_InnerRepository()
        {
            CountByUpcoming();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByCategory_Should_Log()
        {
            CountByCategory();

            log.Verify();
        }

        [Fact]
        public void CountByCategory_Should_Use_InnerRepository()
        {
            CountByCategory();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByTag_Should_Log()
        {
            CountByTag();

            log.Verify();
        }

        [Fact]
        public void CountByTag_Should_Use_InnerRepository()
        {
            CountByTag();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByNew_Should_Log()
        {
            CountByNew();

            log.Verify();
        }

        [Fact]
        public void CountByNew_Should_Use_InnerRepository()
        {
            CountByNew();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByUnapproved_Should_Log()
        {
            CountByUnapproved();

            log.Verify();
        }

        [Fact]
        public void CountByUnapproved_Should_Use_InnerRepository()
        {
            CountByUnapproved();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByPublishable_Should_Log()
        {
            CountByPublishable();

            log.Verify();
        }

        [Fact]
        public void CountByPublishable_Should_Use_InnerRepository()
        {
            CountByPublishable();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountPostedByUser_Should_Log()
        {
            CountPostedByUser();

            log.Verify();
        }

        [Fact]
        public void CountPostedByUser_Should_Use_InnerRepository()
        {
            CountPostedByUser();

            _innerRepository.Verify();
        }

        private void Add()
        {
            _innerRepository.Setup(r => r.Add(It.IsAny<IStory>())).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.Add(CreateStubStory());
        }

        private void Remove()
        {
            _innerRepository.Setup(r => r.Remove(It.IsAny<IStory>())).Verifiable();
            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.Remove(CreateStubStory());
        }

        private void FindById(IStory result)
        {
            _innerRepository.Setup(r => r.FindById(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindById(Guid.NewGuid());
        }

        private void FindByUniqueName(IStory result)
        {
            _innerRepository.Setup(r => r.FindByUniqueName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUniqueName("a-dummy-story");
        }

        private void FindByUrl(IStory result)
        {
            _innerRepository.Setup(r => r.FindByUrl(It.IsAny<string>())).Returns(result).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUrl("http://story.com");
        }

        private void FindPublished(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublished(0, 10);
        }

        private void FindPublishedByCategory(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindPublishedByCategory(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublishedByCategory(Guid.NewGuid(), 0, 10);
        }

        private void FindUpcoming(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindUpcoming(0, 10);
        }

        private void FindNew(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindNew(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindNew(0, 10);
        }

        private void FindUnapproved(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindUnapproved(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindUnapproved(0, 10);
        }

        private void FindPublishable(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4), 0, 10);
        }

        private void FindByTag(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindByTag(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByTag(Guid.NewGuid(), 0, 10);
        }

        private void Search(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.Search("query", 0, 10);
        }

        private void FindPostedByUser(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindPostedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPostedByUser(Guid.NewGuid(), 0, 10);
        }

        private void FindPromotedByUser(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindPromotedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPromotedByUser(Guid.NewGuid(), 0, 10);
        }

        private void FindCommentedByUser(ICollection<IStory> result)
        {
            _innerRepository.Setup(r => r.FindCommentedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindCommentedByUser(Guid.NewGuid(), 0, 10);
        }

        private void CountByPublished()
        {
            _innerRepository.Setup(r => r.CountByPublished()).Returns(10).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByPublished();
        }

        private void CountByUpcoming()
        {
            _innerRepository.Setup(r => r.CountByUpcoming()).Returns(10).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByUpcoming();
        }

        private void CountByCategory()
        {
            _innerRepository.Setup(r => r.CountByCategory(It.IsAny<Guid>())).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByCategory(Guid.NewGuid());
        }

        private void CountByTag()
        {
            _innerRepository.Setup(r => r.CountByTag(It.IsAny<Guid>())).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByTag(Guid.NewGuid());
        }

        private void CountByNew()
        {
            _innerRepository.Setup(r => r.CountByNew()).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByNew();
        }

        private void CountByUnapproved()
        {
            _innerRepository.Setup(r => r.CountByUnapproved()).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByUnapproved();
        }

        private void CountByPublishable()
        {
            _innerRepository.Setup(r => r.CountByPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4));
        }

        private void CountPostedByUser()
        {
            _innerRepository.Setup(r => r.CountPostedByUser(It.IsAny<Guid>())).Returns(0).Verifiable();
            log.Setup(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountPostedByUser(Guid.NewGuid());
        }
    }
}