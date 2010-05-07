namespace Shrinkr.Infrastructure
{
    using System;
    using System.Diagnostics;

    public class EventSubscription<TPayload> : IEventSubscription
    {
        private readonly IDelegateReference actionReference;
        private readonly IDelegateReference filterReference;

        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            Check.Argument.IsNotNull(actionReference, "actionReference");
            Check.Argument.IsNotNull(filterReference, "filterReference");

            if (!(actionReference.Target is Action<TPayload>))
            {
                throw new ArgumentException(TextMessages.InvalidDelegateReferenceType, "actionReference");
            }

            if (!(filterReference.Target is Predicate<TPayload>))
            {
                throw new ArgumentException(TextMessages.InvalidDelegateReferenceType, "filterReference");
            }

            this.actionReference = actionReference;
            this.filterReference = filterReference;
        }

        public Action<TPayload> Action
        {
            [DebuggerStepThrough]
            get
            {
                return (Action<TPayload>) actionReference.Target;
            }
        }

        public Predicate<TPayload> Filter
        {
            [DebuggerStepThrough]
            get
            {
                return (Predicate<TPayload>) filterReference.Target;
            }
        }

        public SubscriptionToken SubscriptionToken
        {
            get;
            set;
        }

        public virtual Action<object[]> GetExecutionStrategy()
        {
            Action<TPayload> action = Action;
            Predicate<TPayload> filter = Filter;

            if (action != null && filter != null)
            {
                return arguments =>
                                   {
                                       TPayload argument = default(TPayload);

                                       if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                                       {
                                           argument = (TPayload) arguments[0];
                                       }

                                       if (filter(argument))
                                       {
                                           InvokeAction(action, argument);
                                       }
                                   };
            }

            return null;
        }

        [DebuggerStepThrough]
        protected virtual void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            action(argument);
        }
    }
}