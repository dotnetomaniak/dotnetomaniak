using System;
using System.Collections.Generic;
using Kigg.DomainObjects;
using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class CachingCategoryRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<ICategoryRepository> _innerRepository;

        public CachingCategoryRepositoryFixture()
        {
            _innerRepository = new Mock<ICategoryRepository>();

            FindAll();
        }

        [Fact]
        public void FindAll_Should_Cache()
        {
            cache.Verify();
        }

        [Fact]
        public void FindAll_Should_Use_InnerRepository()
        {
            _innerRepository.Verify();
        }

        private void FindAll()
        {
            var repository = new CachingCategoryRepository(_innerRepository.Object, 10);

            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<ICollection<ICategory>>(), It.IsAny<DateTime>())).Verifiable();
            _innerRepository.Setup(r => r.FindAll()).Returns(new List<ICategory> { CreateStubCategory() }).Verifiable();

            repository.FindAll();
        }
    }
}