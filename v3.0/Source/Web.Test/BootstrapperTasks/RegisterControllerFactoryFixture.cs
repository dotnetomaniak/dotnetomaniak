using System.Web.Mvc;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;

    public class RegisterControllerFactoryFixture
    {
        private readonly Mock<IControllerFactory> _controllerFactory;
        private readonly IBootstrapperTask _task;

        public RegisterControllerFactoryFixture()
        {
            _controllerFactory = new Mock<IControllerFactory>();

            _task = new RegisterControllerFactory(_controllerFactory.Object);
        }

        [Fact]
        public void Execute_Should_Register_Controller_Factory()
        {
            Assert.DoesNotThrow(() => _task.Execute());
        }
    }
}