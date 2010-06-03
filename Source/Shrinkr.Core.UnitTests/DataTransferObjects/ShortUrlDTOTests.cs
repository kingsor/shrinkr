namespace Shrinkr.UnitTests
{
    using DataTransferObjects;
    using DomainObjects;
    using Extensions;

    using Xunit;

    public class ShortUrlDTOTests
    {
        private readonly Alias alias;
        private readonly ShortUrlDTO dto;

        public ShortUrlDTOTests()
        {
            alias = new Alias { Name = "dtntshtt", ShortUrl = new ShortUrl { Title = "Daily .NET News", Url = "http://dotnetshoutout.com", SpamStatus = SpamStatus.Clean } };
            alias.Visits.AddRange(new[] { new Visit(), new Visit(), new Visit() });

            dto = new ShortUrlDTO(alias, 3, "http://shrinkr.com/1", "http://shrinkr.com/Preview/1");
        }

        [Fact]
        public void Title_should_be_same_as_short_url_title()
        {
            Assert.Equal(alias.ShortUrl.Title, dto.Title);
        }

        [Fact]
        public void Url_should_be_same_as_short_url_url()
        {
            Assert.Equal(alias.ShortUrl.Url, dto.Url);
        }

        [Fact]
        public void Alias_should_be_same_as_alias_name()
        {
            Assert.Equal(alias.Name, dto.Alias);
        }

        [Fact]
        public void CreatedAt_should_be_same_as_alias_createdAt()
        {
            Assert.Equal(alias.CreatedAt, dto.CreatedAt);
        }

        [Fact]
        public void SpamStatus_should_be_same_as_short_url_spam_status()
        {
            Assert.Equal(alias.ShortUrl.SpamStatus, dto.SpamStatus);
        }

        [Fact]
        public void Visits_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal(3, dto.Visits);
        }

        [Fact]
        public void VisitUrl_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("http://shrinkr.com/1", dto.VisitUrl);
        }

        [Fact]
        public void PreviewUrl_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("http://shrinkr.com/Preview/1", dto.PreviewUrl);
        }
    }
}