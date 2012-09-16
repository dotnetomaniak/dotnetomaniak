using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.Repository;

    public class ConnectionStringFixture
    {
        [Fact]
        public void Value_Should_Return_Correct_ConnectionString()
        {
            var configuration = new Mock<IConfigurationManager>();
            configuration.Setup(c => c.GetConnectionString(It.IsAny<string>())).Returns("A Valid ConnectionString");

            var connectionString = new ConnectionString(configuration.Object, "DotNetShoutout");

            Assert.True(connectionString.Value.Length > 0);
        }
    }
}