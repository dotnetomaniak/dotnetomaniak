namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class LoggerWrapper : ILogger
    {
        public void Write(LogEntry logEntry)
        {
            Logger.Write(logEntry);
        }
    }
}