namespace Kigg.Infrastructure
{
    public interface IBackgroundTask
    {
        bool IsRunning
        {
            get;
        }

        void Start();

        void Stop();
    }
}