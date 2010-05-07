namespace Shrinkr.UnitTests
{
    using Services;

    using Xunit;

    public class ValidationTests
    {
        [Fact]
        public void Should_be_able_to_validate_with_single_condition()
        {
            var result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrEmpty(string.Empty), "x", "X cannot be blank").Result();

            Assert.Equal(1, result.RuleViolations.Count);
        }

        [Fact]
        public void Should_be_able_to_validate_with_or_clause()
        {
            var result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrEmpty("foo"), "x", "X cannot be blank")
                                   .Or(() => string.IsNullOrEmpty(string.Empty), "Y", "Y cannot be blank")
                                   .Result();

            Assert.Equal(1, result.RuleViolations.Count);
        }

        [Fact]
        public void Should_be_able_to_validate_with_and_clause()
        {
            var result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrEmpty(string.Empty), "x", "X cannot be blank")
                                   .And(() => string.IsNullOrEmpty(string.Empty), "Y", "Y cannot be blank")
                                   .Result();

            Assert.Equal(2, result.RuleViolations.Count);
        }

        [Fact]
        public void Should_be_able_to_validate_with_both_or_and_clause()
        {
            var result = Validation.Validate<ShortUrlResult>(() => string.IsNullOrEmpty(string.Empty), "x", "X cannot be blank")
                                   .And(() => string.IsNullOrEmpty(string.Empty), "Y", "Y cannot be blank")
                                   .Or(() => string.IsNullOrEmpty(string.Empty), "Z", "Z cannot be blank")
                                   .Result();

            Assert.Equal(2, result.RuleViolations.Count);
        }
    }
}