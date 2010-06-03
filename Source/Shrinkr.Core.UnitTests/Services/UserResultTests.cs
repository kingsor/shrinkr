namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Xunit;

    public class UserResultTests
    {
        [Fact]
        public void Rule_violations_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new UserResult(new List<RuleViolation> { new RuleViolation("foo", "bar") }));
        }

        [Fact]
        public void Default_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new UserResult());
        }

        [Fact]
        public void User_should_be_same_which_is_passed_in_constructor()
        {
            var dto = new UserDTO(new User());
            var userResult = new UserResult(dto);

            Assert.Same(dto, userResult.User);
        }
    }
}