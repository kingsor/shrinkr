namespace Shrinkr.UnitTests
{
    using System;

    using Infrastructure;

    using Xunit;

    public class SystemTimeTests
    {
        [Fact]
        public void Should_be_able_to_set_now()
        {
            Func<DateTime> now = () => DateTime.UtcNow;

            SystemTime.Now = now;

            Assert.Same(now, SystemTime.Now);
        }
    }
}