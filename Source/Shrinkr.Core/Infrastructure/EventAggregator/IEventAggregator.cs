namespace Shrinkr.Infrastructure
{
    public interface IEventAggregator
    {
        TEvent GetEvent<TEvent>() where TEvent : EventBase;
    }
}