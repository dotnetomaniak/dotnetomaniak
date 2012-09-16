using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using DomainObjects;
    using Kigg.LinqToSql.DomainObjects;
    using Kigg.LinqToSql.Repository;

    public class BaseRepositoryFixture : LinqToSqlBaseFixture
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