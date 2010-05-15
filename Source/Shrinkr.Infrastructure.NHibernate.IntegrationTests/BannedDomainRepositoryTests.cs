namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;
    
    using DomainObjects;

    using Xunit;
    using Xunit.Extensions;

    public class BannedDomainRepositoryTests : DatabaseTestBase
    {
        private readonly BannedDomainRepository repository;

        public BannedDomainRepositoryTests()
        {
            repository = new BannedDomainRepository(DatabaseFactory, new QueryFactory(false));
        }

        [Fact]
        public void Add_should_be_able_to_add_banned_domain()
        {
            var domain = new BannedDomain { Name = "mos.es" };

            repository.Add(domain);

            Assert.True(domain.Id >= 0);
        }

        [Fact]
        public void Delete_should_be_able_to_delete_banned_domain()
        {
            var domain = new BannedDomain { Name = "mos.es" };

            repository.Add(domain);

            repository.Delete(domain);

            Assert.False(Database.BannedDomains.Any(b => b.Id == domain.Id));
        }

        [Theory]
        [InlineData("mos.es", "http://mos.es/12", true)]
        [InlineData("mos.es", "http://mOs.Es/12", true)]
        [InlineData("shrn.kr", "http://shrn.kr/12", true)]
        [InlineData("mos.es", "http://www.asp.net/default.aspx", false)]
        public void IsMatching_should_return_correct_value(string domain, string expression, bool expected)
        {
            var bannedDomain = new BannedDomain { Name = domain };

            repository.Add(bannedDomain);

            bool ismatched = repository.IsMatching(expression);

            Assert.Equal(expected, ismatched);
        }
    }
}
