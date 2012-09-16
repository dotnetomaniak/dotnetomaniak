using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogLoggingExceptionHandlerPolicyCreatorFixture
    {
        [Fact]
        public void CreatePolicies_Should_Not_Throw_Exception()
        {
            WeblogLoggingExceptionHandlerPolicyCreator creator = new WeblogLoggingExceptionHandlerPolicyCreator();

            Assert.DoesNotThrow(() => creator.CreatePolicies(new PolicyList(), "dummy", new WeblogLoggingExceptionHandlerData(), new Mock<IConfigurationSource>().Object));
        }
    }
}