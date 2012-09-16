using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class LoggerWrapperFixture
    {
        [Fact]
        public void Write_Should_Thorw_Exception_When_Config_File_Is_Missing()
        {
            Assert.Throws<ConfigurationErrorsException>(() => new LoggerWrapper().Write(new LogEntry()));
        }
    }
}