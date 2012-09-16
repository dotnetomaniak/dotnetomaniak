using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogLoggingExceptionHandlerDataFixture
    {
        private const string LogCategory = "bar";

        private WeblogLoggingExceptionHandlerData _formatterData;

        [Fact]
        public void LogCategory_Should_Be_Empty_String_When_No_Category_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogLoggingExceptionHandlerData();

            Assert.Equal(string.Empty, _formatterData.LogCategory);
        }

        [Fact]
        public void LogCategory_Should_Be_Same_Which_Is_Passed_In_Constructor()
        {
            _formatterData = new WeblogLoggingExceptionHandlerData("foo", LogCategory);

            Assert.Equal(LogCategory, _formatterData.LogCategory);
        }
    }
}