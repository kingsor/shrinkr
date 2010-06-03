namespace Shrinkr.Infrastructure
{
    using System;
    using System.Diagnostics;

    public class EventArgs<TValue> : EventArgs
    {
        [DebuggerStepThrough]
        public EventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value
        { 
            get;
            private set;
        }
    }
}