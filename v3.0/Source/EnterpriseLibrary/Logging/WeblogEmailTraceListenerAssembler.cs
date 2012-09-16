namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.ObjectBuilder2;

    public class WeblogEmailTraceListenerAssembler : TraceListenerAsssembler
    {
        public override TraceListener Assemble(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            WeblogEmailTraceListenerData castedObjectConfiguration = (WeblogEmailTraceListenerData) objectConfiguration;

            ILogFormatter formatter = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

            TraceListener createdObject = new WeblogEmailTraceListener(castedObjectConfiguration.ToAddress, castedObjectConfiguration.FromAddress, castedObjectConfiguration.SubjectLineStarter, castedObjectConfiguration.SubjectLineEnder, castedObjectConfiguration.SmtpServer, castedObjectConfiguration.SmtpPort, castedObjectConfiguration.EnableSsl, castedObjectConfiguration.UserName, castedObjectConfiguration.Password, formatter);

            return createdObject;
        }
    }
}