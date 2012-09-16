using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    public class ControllerContextExtensionFixture
    {
        [Fact]
        public void Url_Should_Return_New_Helper()
        {
            var httpContext = MvcTestHelper.GetHttpContext();

            ControllerContext controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            Assert.NotNull(controllerContext.Url());
        }
    }
}