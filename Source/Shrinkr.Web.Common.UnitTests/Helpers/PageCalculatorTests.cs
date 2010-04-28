namespace Shrinkr.Web.UnitTests
{
    using Xunit;
    using Xunit.Extensions;
    public class PageCalculatorTests
    {
        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 10, 1)]
        [InlineData(6, 10, 1)]
        [InlineData(20, 10, 2)]
        [InlineData(26, 10, 3)]
        public void TotalPage_should_return_correct_result(int total, int itemsPerPage, int expected)
        {
            int actual = PageCalculator.TotalPage(total, itemsPerPage);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, 10, 0)]
        [InlineData(0, 10, 0)]
        [InlineData(1, 10, 0)]
        [InlineData(2, 10, 10)]
        [InlineData(3, 10, 20)]
        [InlineData(1, 5, 0)]
        [InlineData(2, 5, 5)]
        [InlineData(3, 5, 10)]
        public void StartIndex_should_return_correct_result(int? page, int itemsPerPage, int expected)
        {
            int actual = PageCalculator.StartIndex(page, itemsPerPage);
            Assert.Equal(expected, actual);
        }
    }
}
