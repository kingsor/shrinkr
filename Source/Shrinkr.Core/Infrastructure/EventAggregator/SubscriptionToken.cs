namespace Shrinkr.Infrastructure
{
    using System;
    using System.Diagnostics;

    public class SubscriptionToken : IEquatable<SubscriptionToken>
    {
        private readonly Guid token = Guid.NewGuid();

        [DebuggerStepThrough]
        public bool Equals(SubscriptionToken other)
        {
            return (other != null) && Equals(token, other.token);
        }

        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as SubscriptionToken);
        }

        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            return token.GetHashCode();
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return token.ToString();
        }
    }
}