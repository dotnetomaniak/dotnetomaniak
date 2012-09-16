using System;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class ControllerFactoryFixture : BaseFixture
    {
        private readonly IControllerFactory _factory;

        public ControllerFactoryFixture()
        {
            _factory = new ControllerFactoryTestDouble();
        }

        [Fact]
        public void CreateController_Should_Use_IoC_To_Create_Controller()
        {
            resolver.Setup(r => r.Resolve<IController>(It.IsAny<Type>())).Returns((IController) null).Verifiable();

            var httpContext = new HttpContextMock();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            _factory.CreateController(requestContext, "Dummy");

            resolver.Verify();
        }
    }

    public class ControllerFactoryTestDouble : ControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            return typeof(ControllerTestDouble);
            
        }
    }
}