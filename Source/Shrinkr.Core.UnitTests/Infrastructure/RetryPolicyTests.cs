namespace Shrinkr.UnitTests
{
    using System;

    using Infrastructure;

    using Xunit;

    public class RetryPolicyTests
    {
        [Fact]
        public void Retry_should_break_on_success()
        {
            bool flag = false;

            RetryPolicy.Retry(() => flag = true, () => (flag), 2, TimeSpan.Zero);

            Assert.True(flag);
        }

        [Fact]
        public void Retry_should_continue_number_of_specified_times()
        {
            int counter = 0;

            RetryPolicy.Retry(() => counter += 1, () => (false), 3, TimeSpan.FromSeconds(1));

            Assert.Equal(3, counter);
        }
    }
}