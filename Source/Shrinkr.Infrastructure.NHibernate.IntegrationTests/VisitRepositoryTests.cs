namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;

    using Extensions;
    using DomainObjects;

    using Xunit;
    using Xunit.Extensions;

    public class VisitRepositoryTests : DatabaseTestBase
    {
        private readonly VisitRepository repository;

        public VisitRepositoryTests()
        {
            repository = new VisitRepository(DatabaseFactory, new QueryFactory(false));
        }
        
        [Fact]
        public void Add_should_be_able_to_visit()
        {
            var visit = CreateVisit("a");

            Database.Save(visit.Alias.ShortUrl);
            Database.Save(visit.Alias);

            repository.Add(visit);

            Assert.True(visit.Id >= 0);
        }

        [Fact]
        public void Delete_should_be_able_to_visit()
        {
            var visit = CreateVisit("a");

            Database.Save(visit.Alias.ShortUrl);
            Database.Save(visit.Alias);

            repository.Add(visit);

            repository.Delete(visit);

            Assert.False(Database.Visits.Any(b => b.Id == visit.Id));
        }

        [Theory]
        [InlineData("Alias", "alias", 1, false)]
        [InlineData("alias", "alias", 1, false)]
        [InlineData("Alias", "alias", 0, true)]
        [InlineData("alias", "Alias", 0, true)]
        [InlineData("Alias", "Alias", 1, true)]
        [InlineData("Alias", "nhalias", 0, false)]
        [InlineData("Alias", "AliasNH", 0, true)]
        public void IsMatching_should_return_correct_value(string alias, string expression, int expectedCount, bool useCaseSensitive)
        {
            var queryFactory = new QueryFactory(useCaseSensitive);

            Visit visit = CreateVisit(alias);
            Database.Save(visit.Alias.ShortUrl);
            Database.Save(visit.Alias);

            var repo = new VisitRepository(DatabaseFactory, queryFactory);
            
            repo.Add(visit);

            int count = repo.Count(expression);

            Assert.Equal(expectedCount, count);
        }

        private static ShortUrl CreateShortUrl()
        {
            const string url = "http://www.somedomain.com";
            return new ShortUrl
            {
                Domain = "somedomain.com",
                Url = url,
                Title = "Some title",
                Hash = url.Hash(),
            };
        }

        private static Alias CreateAlias(string alias)
        {
            return new Alias
            {
                Name = alias,
                ShortUrl = CreateShortUrl(),
                IPAddress = "127.0.0.1",
                CreatedAt = SystemTime.Now()
            };
        }

        private static Visit CreateVisit(string alias)
        {
            return new Visit
            {
                Alias = CreateAlias(alias),
                CreatedAt = SystemTime.Now(),
                IPAddress = "127.0.0.1",
                Referrer = new Referrer { Domain = "somedomain.com" }
            };
        }
    }
}
