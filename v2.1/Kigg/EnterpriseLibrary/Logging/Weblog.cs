namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class Weblog : ILog
    {
        private readonly string _traceCategory;
        private readonly ILogger _logger;
        private readonly string _exceptionPolicyName;
        private readonly IExceptionPolicy _exceptionPolicy;
        private readonly int _frameToSkip;

        public Weblog(string traceCategory, ILogger logger, string exceptionPolicyName, IExceptionPolicy exceptionPolicy, int frameToSkip)
        {
            Check.Argument.IsNotNull(traceCategory, "traceCategory");
            Check.Argument.IsNotNull(logger, "logger");
            Check.Argument.IsNotNull(exceptionPolicyName, "exceptionPolicyName");
            Check.Argument.IsNotNull(exceptionPolicy, "exceptionPolicy");
            Check.Argument.IsNotNegativeOrZero(frameToSkip, "frameToSkip");

            _traceCategory = traceCategory;
            _logger = logger;
            _exceptionPolicyName = exceptionPolicyName;
            _exceptionPolicy = exceptionPolicy;
            _frameToSkip = frameToSkip;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Info(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Write(BuildEntry(message, TraceEventType.Information));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Warning(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Write(BuildEntry(message, TraceEventType.Warning));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Error(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            Write(BuildEntry(message, TraceEventType.Error));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Exception(Exception exception)
        {
            Check.Argument.IsNotNull(exception, "exception");

            Exception exceptionToThrow;

            if (_exceptionPolicy.HandleException(exception, _exceptionPolicyName, out exceptionToThrow))
            {
                throw exceptionToThrow ?? exception;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private LogEntry BuildEntry(string message, TraceEventType severity)
        {
            string namespaceName;
            string className;
            string methodSignature;

            WeblogEntry.GetMethodDetails(_frameToSkip, out namespaceName, out className, out methodSignature);

            return CreateEntry(message, severity, namespaceName, className, methodSignature);
        }

        private LogEntry CreateEntry(string message, TraceEventType severity, string namespaceName, string className, string methodSignature)
        {
            return new WeblogEntry(message, _traceCategory, severity, namespaceName, className, methodSignature);
        }

        private void Write(LogEntry logEntry)
        {
            _logger.Write(logEntry);
        }
    }
}