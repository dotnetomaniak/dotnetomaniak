using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    
    using Kigg.EF.Repository;

    public class DatabaseFactoryFixture
    {
        private readonly IDatabaseFactory _factory;

        public DatabaseFactoryFixture()
        {
            var configMngr = new Mock<IConfigurationManager>();
            configMngr.Setup(c => c.GetConnectionString("KiGG")).Returns("Data Source=.\\sqlexpress;Initial Catalog=KiGG;Integrated Security=True;MultipleActiveResultSets=False");
            configMngr.Setup(c => c.GetProviderName("KiGG")).Returns("System.Data.SqlClient");
            var connectionString = new ConnectionString(configMngr.Object, "KiGG", ".\\EDM");

            _factory = new DatabaseFactory(connectionString);
        }

        [Fact]
        public void Get_Should_Return_The_Same_Database()
        {
            var db1 = _factory.Create();
            var db2 = _factory.Create();

            Assert.Same(db1, db2);
        }

        [Fact]
        public void Accessing_Database_After_Dispose_Should_Throw_Exception()
        {
            var db = _factory.Create();
            _factory.Dispose();

            Assert.Throws<ObjectDisposedException>(() => db.SubmitChanges());
        }
    }
}