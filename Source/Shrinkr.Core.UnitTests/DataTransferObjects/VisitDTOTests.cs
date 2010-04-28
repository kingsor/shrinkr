namespace Shrinkr.UnitTests
{
    using DataTransferObjects;
    using DomainObjects;

    using Xunit;

    public class VisitDTOTests
    {
        private readonly Visit visit;
        private readonly VisitDTO dto;

        public VisitDTOTests()
        {
            visit = new Visit {
                                IPAddress = "192.168.0.2",
                                Referrer = new Referrer { Domain = "twitter.com", Url = "http://twitter.com/manzurrashid" },
                                Alias = new Alias
                                        {
                                            Name = "shrnkr",
                                            IPAddress = "192.168.0.1",
                                            ShortUrl = new ShortUrl
                                                       {
                                                           Url = "http://shrinkr.com",
                                                           Title = "Kool Url Shrinking Service"
                                                       }
                                        }
                              };

            dto = new VisitDTO(visit);
        }

        [Fact]
        public void Title_should_be_same_as_short_url_title()
        {
            Assert.Equal(visit.Alias.ShortUrl.Title, dto.Title);
        }

        [Fact]
        public void Url_should_be_same_as_short_url_url()
        {
            Assert.Equal(visit.Alias.ShortUrl.Url, dto.Url);
        }

        [Fact]
        public void Alias_should_be_same_as_alias_name()
        {
            Assert.Equal(visit.Alias.Name, dto.Alias);
        }

        [Fact]
        public void IPAddress_should_be_same_as_visit_ipaddress()
        {
            Assert.Equal(visit.IPAddress, dto.IPAddress);
        }

        [Fact]
        public void ReferrerDomain_should_be_same_as_visit_referrer_domain()
        {
            Assert.Equal(visit.Referrer.Domain, dto.ReferrerDomain);
        }

        [Fact]
        public void ReferrerUrl_should_be_same_as_visit_referrer_url()
        {
            Assert.Equal(visit.Referrer.Url, dto.ReferrerUrl);
        }

        [Fact]
        public void CreatedAt_should_be_same_as_visit_createdAt()
        {
            Assert.Equal(visit.CreatedAt, dto.CreatedAt);
        }
    }
}