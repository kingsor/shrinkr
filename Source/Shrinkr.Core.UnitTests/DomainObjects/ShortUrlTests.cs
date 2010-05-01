namespace Shrinkr.UnitTests
{
    using DomainObjects;

    using Xunit;

    public class ShortUrlTests
    {
        private readonly ShortUrl shortUrl;

        public ShortUrlTests()
        {
            shortUrl = new ShortUrl();
        }

        [Fact]
        public void Aliases_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(shortUrl.Aliases);
        }
    }
}