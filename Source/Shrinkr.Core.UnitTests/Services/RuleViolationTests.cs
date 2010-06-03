namespace Shrinkr.UnitTests
{
    using Services;

    using Xunit;

    public class RuleViolationTests
    {
        private readonly RuleViolation ruleViolation;

        public RuleViolationTests()
        {
            ruleViolation = new RuleViolation("foo", "bar");
        }

        [Fact]
        public void ParameterName_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("foo", ruleViolation.ParameterName);
        }

        [Fact]
        public void ErrorMessage_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("bar", ruleViolation.ErrorMessage);
        }
    }
}