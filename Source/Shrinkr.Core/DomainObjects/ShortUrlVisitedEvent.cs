namespace Shrinkr.DomainObjects
{
    using Infrastructure;

    public class ShortUrlVisitedEvent : EventBase<EventArgs<Visit>>
    {
    }
}