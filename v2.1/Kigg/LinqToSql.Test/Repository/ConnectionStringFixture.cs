using Moq;
using Xunit;

namespace Kigg.Infrastructure.LinqToSql.Test
{
    using Repository.LinqToSql;

    public class ConnectionStringFixture
    {
        [Fact]
        public void Value_Should_Return_Correct_ConnectionString()
        {
            var configuration = new Mock<IConfigurationManager>();
            configuration.Expect(c => c.ConnectionStrings(It.IsAny<string>())).Returns("A Valid ConnectionString");

            var connectionString = new ConnectionString(configuration.Object, "DotNetShoutout");

            Assert.True(connectionString.Value.Length > 0);
        }
    }
}