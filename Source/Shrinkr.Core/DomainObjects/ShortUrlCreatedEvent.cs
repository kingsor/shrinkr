namespace Shrinkr.DomainObjects
{
    using Infrastructure;

    public class ShortUrlCreatedEvent : EventBase<EventArgs<Alias>>
    {
    }
}