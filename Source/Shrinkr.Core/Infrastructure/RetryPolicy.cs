namespace Shrinkr.Infrastructure
{
    using System;
    using System.Threading;

    public static class RetryPolicy
    {
        public static void Retry(Action action, Func<bool> successCondition, int numberOfRetries, TimeSpan interval)
        {
            Check.Argument.IsNotNull(action, "action");
            Check.Argument.IsNotNull(successCondition, "successCondition");
            Check.Argument.IsNotZeroOrNegative(numberOfRetries, "numberOfRetries");

            do
            {
                action();

                if (successCondition())
                {
                    break;
                }

                if (interval > TimeSpan.Zero)
                {
                    Thread.Sleep(interval);
                }
            }
            while (numberOfRetries-- > 1);
        }
    }
}