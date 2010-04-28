namespace Shrinkr.UnitTests
{
    using Extensions;

    using Xunit;

    public class PagedResultTests
    {
        private const int Total = 100;

        private readonly string[] Languages = new[] {"C#", "VB.NET", "Ruby", "Java", "C++", "Python", "Perl", "PHP"};

        private readonly PagedResult<string> pagedResult;

        public PagedResultTests()
        {
            pagedResult = new PagedResult<string>(Languages, Total);
        }

        [Fact]
        public void Result_should_contain_all_the_items_which_are_passed_in_constructor()
        {
            Languages.Each(language => Assert.Contains(language, pagedResult.Result));
        }

        [Fact]
        public void Total_should_be_the_same_which_is_passed_in_constructor()
        {
            Assert.Equal(Total, pagedResult.Total);
        }

        [Fact]
        public void Result_should_be_empty_when_nothing_is_passed_in_constructor()
        {
            Assert.Empty(new PagedResult<int>().Result);
        }

        [Fact]
        public void IsEmpty_should_be_true_when_result_is_empty()
        {
            Assert.True(new PagedResult<string>().IsEmpty);
        }

        [Fact]
        public void IsEmpty_should_be_true_when_total_is_zero()
        {
            Assert.True(new PagedResult<string>(Languages, 0).IsEmpty);
        }
    }
}