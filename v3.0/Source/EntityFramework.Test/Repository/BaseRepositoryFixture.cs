using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using DomainObjects;
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class BaseRepositoryFixture : EfBaseFixture
    {
        private readonly Mock<BaseRepository<IEntity, Category>> _repository;

        public BaseRepositoryFixture()
        {
            _repository = new Mock<BaseRepository<IEntity, Category>>(database.Object);
        }

        [Fact]
        public void Database_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            Assert.Same(database.Object, _repository.Object.Database);
        }
    }
}