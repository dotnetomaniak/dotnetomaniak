using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.ObjectBuilder2;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogLoggingExceptionHandlerAssemblerFixture
    {
        private readonly IExceptionHandler _hanadler;

        public WeblogLoggingExceptionHandlerAssemblerFixture()
        {
            var context = new Mock<IBuilderContext>();
            var formatterData = new WeblogLoggingExceptionHandlerData();
            var configuration = new Mock<IConfigurationSource>();
            var cache = new ConfigurationReflectionCache();

            var assembler = new WeblogLoggingExceptionHandlerAssembler();

            _hanadler = assembler.Assemble(context.Object, formatterData, configuration.Object, cache);
        }

        [Fact]
        public void Assemble_Should_Return_New_WeblogLoggingExceptionHandler()
        {
            Assert.NotNull(_hanadler);
        }

        [Fact]
        public void Assemble_Should_Return_Correct_WeblogLoggingExceptionHandler_Type()
        {
            Assert.IsType<WeblogLoggingExceptionHandler>(_hanadler);
        }
    }
}