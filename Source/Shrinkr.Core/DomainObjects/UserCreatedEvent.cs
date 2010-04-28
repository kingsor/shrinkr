namespace Shrinkr.DomainObjects
{
    using Infrastructure;

    public class UserCreatedEvent : EventBase<EventArgs<User>>
    {
    }
}