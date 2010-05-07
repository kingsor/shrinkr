namespace Shrinkr.Infrastructure
{
    public class ApiSettings
    {
        public ApiSettings(bool allowed, int dailyLimit)
        {
            Check.Argument.IsNotNegative(dailyLimit, "dailyLimit");

            Allowed = allowed;
            DailyLimit = dailyLimit;
        }

        public bool Allowed
        {
            get;
            internal set;
        }

        public int DailyLimit
        {
            get;
            internal set;
        }
    }
}