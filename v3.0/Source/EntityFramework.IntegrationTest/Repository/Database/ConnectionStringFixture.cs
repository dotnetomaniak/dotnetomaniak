using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;

    public class ConnectionStringFixture
    {
        private readonly ConfigurationManagerWrapper _configManager;
        public ConnectionStringFixture()
        {
            _configManager = new ConfigurationManagerWrapper();
        }

        [Fact]
        public void Initialize_ConnectionString_With_ConfigurationManager_Should_Succeed()
        {
            #pragma warning disable 168
            var connectionString = new ConnectionString(_configManager, "KiGG");
            #pragma warning restore 168
        }

#if(SqlServer)
        [Fact]
        public void Connecting_To_SqlServer_Database_Using_ConnectionString_Should_Succeed()
        {
            //Connection string must be provided correctlly in app.config
            var connectionString = new ConnectionString(_configManager, "KiGG", ".\\EDM", "DomainObjects.SqlServer");
            using(var database = new Database(connectionString.Value))
            {
                #pragma warning disable 168
                var user = database.UserDataSource.FirstOrDefault();
                #pragma warning restore 168
            }
        }
#endif

#if(MySql)
        [Fact]
        public void Connecting_To_MySql_Database_Using_ConnectionString_Should_Succeed()
        {
            //Connection string must be provided correctlly in app.config
            var connectionString = new ConnectionString(_configManager, "KiGGMySql", ".\\EDM", "DomainObjects.MySql");
            using (var database = new Database(connectionString.Value))
            {
                #pragma warning disable 168
                var user = database.UserDataSource.FirstOrDefault();
                #pragma warning restore 168
            }
        }
#endif
    }
}
