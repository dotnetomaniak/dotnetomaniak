using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EnterpriseLibrary.Test
{
    public class WeblogEmailTraceListenerAssemblerFixture
    {
        private readonly TraceListener _traceListner;

        public WeblogEmailTraceListenerAssemblerFixture()
        {
            var context = new Mock<IBuilderContext>();
            var listenerData = new WeblogEmailTraceListenerData();
            var configuration = new Mock<IConfigurationSource>();
            var cache = new ConfigurationReflectionCache();

            var assembler = new WeblogEmailTraceListenerAssembler();

            _traceListner = assembler.Assemble(context.Object, listenerData, configuration.Object, cache);
        }

        [Fact]
        public void Assemble_Should_Return_New_TraceListner()
        {
            Assert.NotNull(_traceListner);
        }

        [Fact]
        public void Assemble_Should_Return_Correct_WeblogEmailTraceListener_Type()
        {
            Assert.IsType<WeblogEmailTraceListener>(_traceListner);
        }
    }
}