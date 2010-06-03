namespace Shrinkr.UnitTests
{
    using Xunit;

    public class CultureTests
    {
        [Fact]
        public void Current_should_not_be_null()
        {
            Assert.NotNull(Culture.Current);
        }

        [Fact]
        public void Invariant_should_not_be_null()
        {
            Assert.NotNull(Culture.Invariant);
        }
    }
}