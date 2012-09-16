using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogEmailMessageFixture
    {
        private readonly WeblogEmailMessage _emailMessage;

        public WeblogEmailMessageFixture()
        {
            _emailMessage = new WeblogEmailMessage("kazimanzurrashid@gmail.com", "admin@dotnetshoutout.com", string.Empty, string.Empty, "smtp.gmail.com", 25, true, "kigg.webmaster@gmail.com", "pwd", "message", new Mock<ILogFormatter>().Object);
        }

        [Fact]
        public void Constructor_With_ConfigurationObject_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new WeblogEmailMessage(new WeblogEmailTraceListenerData(), new LogEntry(), new Mock<ILogFormatter>().Object));
        }

        [Fact]
        public void Constructor_With_LogEntry_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new WeblogEmailMessage("kazimanzurrashid@gmail.com", "admin@dotnetshoutout.com", string.Empty, string.Empty, string.Empty, 0, true, string.Empty, string.Empty, new LogEntry(), new Mock<ILogFormatter>().Object));
        }

        [Fact]
        public void Constructor_With_Message_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new WeblogEmailMessage("kazimanzurrashid@gmail.com", "admin@dotnetshoutout.com", string.Empty, string.Empty, string.Empty, 0, true, string.Empty, string.Empty, "message", new Mock<ILogFormatter>().Object));
        }

        [Fact]
        public void Send_Should_Not_Throw_Exception_When_Running_In_Different_Thread()
        {
            Assert.DoesNotThrow(() => _emailMessage.Send());
        }

        [Fact]
        public void Send_Message_Should_Not_Throw_Exception_When_Running_In_Different_Thread()
        {
            Assert.DoesNotThrow(() => _emailMessage.SendMessage(new MailMessage("kigg.webmaster@gmail.com", "kazimanzurrashid@gmail.com")));
            System.Threading.Thread.Sleep(5000);
        }
    }
}