namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
    using Microsoft.Practices.ObjectBuilder2;

    public class WeblogLoggingExceptionHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            WeblogLoggingExceptionHandlerData castedObjectConfiguration = (WeblogLoggingExceptionHandlerData) objectConfiguration;
            WeblogLoggingExceptionHandler createdObject = new WeblogLoggingExceptionHandler(castedObjectConfiguration.LogCategory, new LoggerWrapper());

            return createdObject;
        }
    }
}