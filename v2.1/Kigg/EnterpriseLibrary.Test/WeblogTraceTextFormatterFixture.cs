using System.Diagnostics;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogTraceTextFormatterFixture
    {
        private const string Template = "Bla Bla";

        private WeblogTraceTextFormatter _formatter;

        public WeblogTraceTextFormatterFixture()
        {
            _formatter = new WeblogTraceTextFormatter();
        }

        [Fact]
        public void Template_Should_Be_Set_To_Default_Template_When_No_Template_Is_Passed_In_Constructor()
        {
            Assert.Equal(WeblogTraceTextFormatter.DefaultTextFormat, _formatter.Template);
        }

        [Fact]
        public void Template_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            _formatter = new WeblogTraceTextFormatter(Template);

            Assert.Equal(Template, _formatter.Template);
        }

        [Fact]
        public void Setting_And_Getting_Template_Should_Return_The_Same_Value()
        {
            _formatter.Template = Template;

            Assert.Equal(Template, _formatter.Template);
        }

        [Fact]
        public void Format_Should_Return_String_Representation_Of_LogEntry()
        {
            var logEntry = new WeblogEntry(new Mock<HttpContextBase>().Object, "Dummy Message", "All", TraceEventType.Information, null, null, null);
            var response = _formatter.Format(logEntry);

            Assert.True(response.Length > 0);
        }
    }
}