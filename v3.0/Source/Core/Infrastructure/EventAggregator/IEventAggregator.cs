namespace Kigg.Infrastructure
{
    public interface IEventAggregator
    {
        TEventType GetEvent<TEventType>() where TEventType : BaseEvent;
    }
}