namespace Shrinkr.DomainObjects
{
    using System.Diagnostics;

    public class ApiSetting
    {
        public const int InfiniteLimit = 0;

        private int? dailyLimit;

        public virtual string Key
        {
            get;
            set;
        }

        public virtual bool? Allowed
        {
            get;
            set;
        }

        public virtual int? DailyLimit
        {
            [DebuggerStepThrough]
            get
            {
                return dailyLimit;
            }

            [DebuggerStepThrough]
            set
            {
                if (value.HasValue)
                {
                    Check.Argument.IsNotNegative(value.Value, "value");
                }

                dailyLimit = value;
            }
        }
    }
}