namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public interface ILogger
    {
        void Write(LogEntry logEntry);
    }
}