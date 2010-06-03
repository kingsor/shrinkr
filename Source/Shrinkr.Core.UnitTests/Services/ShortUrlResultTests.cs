namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Xunit;

    public class ShortUrlResultTests
    {
        private readonly ShortUrlDTO dto;
        private readonly ShortUrlResult result;

        public ShortUrlResultTests()
        {
            var alias = new Alias { Name = "dtntshtt", ShortUrl = new ShortUrl { Title = "Daily .NET News", Url = "http://dotnetshoutout.com" } };
            dto = new ShortUrlDTO(alias, 3, "http://shrinkr.com/1", "http://shrinkr.com/Preview/1");

            result = new ShortUrlResult(dto);
        }

        [Fact]
        public void Rule_violations_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new ShortUrlResult(new List<RuleViolation> { new RuleViolation("foo", "bar") }));
        }

        [Fact]
        public void Default_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new ShortUrlResult());
        }

        [Fact]
        public void ShortUrl_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Same(dto, result.ShortUrl);
        }
    }
}