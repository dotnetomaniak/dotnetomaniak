using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogEmailTraceListenerDataFixture
    {
        private readonly WeblogEmailTraceListenerData _listenerData;

        public WeblogEmailTraceListenerDataFixture()
        {
            _listenerData = new WeblogEmailTraceListenerData();
        }

        [Fact]
        public void Setting_And_Getting_Name_Returns_The_Same_Value()
        {
            _listenerData.UserName = "user@domain.com";

            Assert.Equal("user@domain.com", _listenerData.UserName);
        }

        [Fact]
        public void Setting_And_Getting_Password_Should_Return_The_Same_Value()
        {
            _listenerData.Password = "mypwd";

            Assert.Equal("mypwd", _listenerData.Password);
        }

        [Fact]
        public void Setting_And_Getting_EnableSsl_Should_Return_The_Same_Value()
        {
            _listenerData.EnableSsl = true;

            Assert.True(_listenerData.EnableSsl);
        }
    }
}