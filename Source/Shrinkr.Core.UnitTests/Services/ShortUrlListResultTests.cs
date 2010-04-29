namespace Shrinkr.UnitTests
{
    using System.Collections.Generic;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Xunit;

    public class ShortUrlListResultTests
    {
        private readonly ShortUrlDTO dto;
        private readonly ShortUrlListResult result;

        public ShortUrlListResultTests()
        {
            var alias = new Alias { Name = "dtntshtt", ShortUrl = new ShortUrl { Title = "Daily .NET News", Url = "http://dotnetshoutout.com" } };
            dto = new ShortUrlDTO(alias, 3, "http://shrinkr.com/1", "http://shrinkr.com/Preview/1");

            var pagedResult = new PagedResult<ShortUrlDTO>(new List<ShortUrlDTO> { dto }, 1);
            result = new ShortUrlListResult(pagedResult);
        }

        [Fact]
        public void Rule_violations_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new ShortUrlListResult(new List<RuleViolation> { new RuleViolation("foo", "bar") }));
        }

        [Fact]
        public void Default_constructor_should_not_throw_exception()
        {
            Assert.DoesNotThrow(() => new ShortUrlListResult());
        }

        [Fact]
        public void ShortUrl_should_exists_which_is_passed_in_constructor()
        {
            Assert.Contains(dto, result.ShortUrls);
        }

        [Fact]
        public void Total_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal(1, result.Total);
        }
    }
}