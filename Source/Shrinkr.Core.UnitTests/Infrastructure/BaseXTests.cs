namespace Shrinkr.UnitTests
{
    using System;

    using Infrastructure;

    using Xunit;
    using Xunit.Extensions;

    public class BaseXTests
    {
        private readonly IBaseX basex;

        public BaseXTests()
        {
            basex = new BaseX(BaseType.BaseSixtyTwo);
        }

        [Theory]
        [InlineData(10000)]
        public void Should_be_able_to_encode_and_decode(int loopCount)
        {
            for (int i = 1; i <= loopCount; i++)
            {
                var rnd = new Random();
                var value = rnd.Next(0, int.MaxValue);

                var encoded = basex.Encode(value);
                var decoded = basex.Decode(encoded);

                Assert.Equal(value, decoded);
            }
        }

        [Theory]
        [InlineData("1234?!", false)]
        [InlineData("1df1as", true)]
        public void IsValid_should_return_correct_value(string value, bool result)
        {
            Assert.Equal(result, basex.IsValid(value));
        }
    }
}