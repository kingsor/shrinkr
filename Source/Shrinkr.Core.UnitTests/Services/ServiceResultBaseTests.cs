namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using Services;

    using Moq;
    using Xunit;

    public class ServiceResultBaseTests
    {
        private readonly RuleViolation ruleViolation;
        private readonly Mock<ServiceResultBase> serviceResult;

        public ServiceResultBaseTests()
        {
            ruleViolation = new RuleViolation("foo", "bar");
            serviceResult = new Mock<ServiceResultBase>(new List<RuleViolation> { ruleViolation });
        }

        [Fact]
        public void RuleViolation_should_exists_which_is_passed_in_constructor()
        {
            Assert.Contains(ruleViolation, serviceResult.Object.RuleViolations);
        }
    }
}