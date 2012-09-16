using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogEmailTraceListenerFixture
    {
        private readonly WeblogEmailTraceListenerTestDouble _emailListner;

        public WeblogEmailTraceListenerFixture()
        {
            _emailListner = new WeblogEmailTraceListenerTestDouble("kazimanzurrashid@gmail.com", "kigg.webmaster@gmail.com", string.Empty, string.Empty, string.Empty, 0, true, string.Empty, string.Empty, new Mock<ILogFormatter>().Object);
        }

        [Fact]
        public void Write_Should_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => _emailListner.Write("foo"));
        }

        [Fact]
        public void GetAttributes_Should_Return_New_Attributes()
        {
            string[] attributes = _emailListner.GetAttributes();

            Assert.Contains("enableSsl", attributes);
            Assert.Contains("userName", attributes);
            Assert.Contains("password", attributes);
        }

        [Fact]
        public void TraceData_Should_Not_Throw_Exception_For_LogEntry()
        {
            Assert.DoesNotThrow(() => _emailListner.TraceData(new TraceEventCache(), string.Empty, TraceEventType.Error, -1, new LogEntry()));
        }

        [Fact]
        public void TraceData_Should_Not_Throw_Exception_For_String()
        {
            Assert.DoesNotThrow(() => _emailListner.TraceData(new TraceEventCache(), string.Empty, TraceEventType.Error, -1, "foo"));
        }

        [Fact]
        public void TraceData_Should_Not_Throw_Exception_For_Others()
        {
            Assert.DoesNotThrow(() => _emailListner.TraceData(new TraceEventCache(), string.Empty, TraceEventType.Error, -1, new object()));
        }

        [Fact]
        public void TraceData_Should_Not_Do_Aanything_When_Filter_Is_Set_To_Should_Not_Trace()
        {
            var filter = new Mock<TraceFilter>();
            filter.Setup(f => f.ShouldTrace(It.IsAny<TraceEventCache>(), It.IsAny<string>(), It.IsAny<TraceEventType>(), It.IsAny<int>(), null, null, It.IsAny<LogEntry>(), null)).Returns(false);

            _emailListner.Filter = filter.Object;

            _emailListner.TraceData(new TraceEventCache(), string.Empty, TraceEventType.Error, -1, new LogEntry());
        }
    }

    public class WeblogEmailTraceListenerTestDouble : WeblogEmailTraceListener
    {
        public WeblogEmailTraceListenerTestDouble(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, bool enableSsl, string userName, string password, ILogFormatter formatter) : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, enableSsl, userName, password, formatter)
        {
        }

        public string[] GetAttributes()
        {
            return base.GetSupportedAttributes();
        }
    }
}