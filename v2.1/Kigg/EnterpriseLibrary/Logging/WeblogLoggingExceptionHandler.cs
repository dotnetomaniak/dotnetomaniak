namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    [ConfigurationElementType(typeof(WeblogLoggingExceptionHandlerData))]
    public class WeblogLoggingExceptionHandler : IExceptionHandler
    {
        private readonly string _logCategory;
        private readonly ILogger _logger;

        public WeblogLoggingExceptionHandler(string logCategory, ILogger logger)
        {
            _logCategory = logCategory;
            _logger = logger;
        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            Exception e = exception;

            while (e != null)
            {
                string namespaceName;
                string className;
                string methodSignature;

                WeblogEntry.GetMethodDetails(11, out namespaceName, out className, out methodSignature);
                LogEntry log = CreateEntry(e, namespaceName, className, methodSignature);

                _logger.Write(log);

                e = e.InnerException;
            }

            return exception;
        }

        private LogEntry CreateEntry(Exception e, string namespaceName, string className, string methodSignature)
        {
            return new WeblogEntry(e.Message, _logCategory, TraceEventType.Critical, namespaceName, className, methodSignature);
        }
    }
}