using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Kigg.LinqToSql.DomainObjects;
    using Kigg.LinqToSql.Repository;

    public class DatabaseFactoryFixture
    {
        private readonly IDatabaseFactory _factory;

        public DatabaseFactoryFixture()
        {
            var connectionString = new Mock<IConnectionString>();
            connectionString.SetupGet(c => c.Value).Returns("foo");

            _factory = new DatabaseFactory(connectionString.Object);
        }

        [Fact]
        public void Get_Should_Return_The_Same_Database()
        {
            var db1 = _factory.Get();
            var db2 = _factory.Get();

            Assert.Same(db1, db2);
        }

        [Fact]
        public void Accessing_Database_After_Dispose_Should_Throw_Exception()
        {
            var db = _factory.Get();
            _factory.Dispose();

            Assert.Throws<ObjectDisposedException>(() => db.Insert(new Story()));
        }
    }
}