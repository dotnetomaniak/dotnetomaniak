using Xunit;

namespace Kigg.Web.Test
{
    public class BaseHttpModuleFixture
    {
        private readonly HttpContextMock _httpContext;
        private readonly BaseHttpModule _module;

        public BaseHttpModuleFixture()
        {
            _httpContext = MvcTestHelper.GetHttpContext();
            _module = new BaseHttpModuleTestDouble();
        }

        [Fact]
        public void OnBeginRequest_Should_Do_Nothing()
        {
            _module.OnBeginRequest(_httpContext.Object);
        }

        [Fact]
        public void OnError_Should_Do_Nothing()
        {
            _module.OnError(_httpContext.Object);
        }

        [Fact]
        public void OnEndRequest_Should_Do_Nothing()
        {
            _module.OnEndRequest(_httpContext.Object);
        }
    }

    public class BaseHttpModuleTestDouble : BaseHttpModule
    {
    }
}