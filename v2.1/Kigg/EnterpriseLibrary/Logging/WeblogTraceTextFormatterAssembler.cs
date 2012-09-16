namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.ObjectBuilder2;

    public class WeblogTraceTextFormatterAssembler : IAssembler<ILogFormatter, FormatterData>
    {
        public ILogFormatter Assemble(IBuilderContext context, FormatterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            WeblogTraceTextFormatterData castedObjectConfiguration = (WeblogTraceTextFormatterData) objectConfiguration;

            ILogFormatter createdObject = new WeblogTraceTextFormatter(castedObjectConfiguration.Template);

            return createdObject;
        }
    }
}