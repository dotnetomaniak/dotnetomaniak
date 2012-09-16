using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.ObjectBuilder2;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogTraceTextFormatterAssemblerFixture
    {
        private readonly ILogFormatter _formatter;

        public WeblogTraceTextFormatterAssemblerFixture()
        {
            var context = new Mock<IBuilderContext>();
            var formatterData = new WeblogTraceTextFormatterData();
            var configuration = new Mock<IConfigurationSource>();
            var cache = new ConfigurationReflectionCache();

            var assembler = new WeblogTraceTextFormatterAssembler();

            _formatter = assembler.Assemble(context.Object, formatterData, configuration.Object, cache);
        }

        [Fact]
        public void Assemble_Should_Return_New_WeblogTraceTextFormatter()
        {
            Assert.NotNull(_formatter);
        }

        [Fact]
        public void Assemble_Should_Return_Correct_WeblogTraceTextFormatter_Type()
        {
            Assert.IsType<WeblogTraceTextFormatter>(_formatter);
        }
    }
}