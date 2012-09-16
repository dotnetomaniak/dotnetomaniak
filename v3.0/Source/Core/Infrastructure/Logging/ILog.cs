namespace Kigg.Infrastructure
{
    using System;

    public interface ILog
    {
        void Info(string message);

        void Warning(string message);

        void Error(string message);

        void Exception(Exception exception);
    }
}