using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class CachingUserRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<IUserRepository> _innerRepository;
        private readonly CachingUserRepository _repository;

        public CachingUserRepositoryFixture()
        {
            _innerRepository = new Mock<IUserRepository>();

            _repository = new CachingUserRepository(_innerRepository.Object, 20, 5, 5);
        }

        [Fact]
        public void FindScoreById_Should_Cache()
        {
            FindByScore();

            cache.Verify();
        }

        [Fact]
        public void FindScoreById_Should_Use_InnerRepository()
        {
            FindByScore();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindTop_Should_Cache()
        {
            FindTop();

            cache.Verify();
        }

        [Fact]
        public void FindTop_Should_Use_InnerRepository()
        {
            FindTop();

            _innerRepository.Verify();
        }

        private void FindByScore()
        {
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>())).Verifiable();

            _innerRepository.Expect(r => r.FindScoreById(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(5).Verifiable();

            _repository.FindScoreById(Guid.NewGuid(), SystemTime.Now().AddHours(-4), SystemTime.Now());
        }

        private void FindTop()
        {
            cache.Expect(c => c.Set(It.IsAny<string>(), It.IsAny<PagedResult<IUser>>(), It.IsAny<DateTime>())).Verifiable();

            _innerRepository.Expect(r => r.FindTop(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<IUser>(new List<IUser> { CreateStubUser() }, 1)).Verifiable();

            _repository.FindTop(SystemTime.Now().AddHours(-4), SystemTime.Now(), 0, 10);
        }
    }
}