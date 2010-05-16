namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;
    
    using DomainObjects;

    using Xunit;
    using Xunit.Extensions;

    public class ReservedAliasRepositoryTests : DatabaseTestBase
    {
        private readonly ReservedAliasRepository repository;

        public ReservedAliasRepositoryTests()
        {
            repository = new ReservedAliasRepository(DatabaseFactory, new QueryFactory(false));
        }

        [Fact]
        public void Add_should_be_able_to_reserved_alias()
        {
            var reservedAlias = new ReservedAlias { Name = "Api" };

            repository.Add(reservedAlias);

            Assert.True(reservedAlias.Id >= 0);
        }

        [Fact]
        public void Delete_should_be_able_to_delete_reserved_alias()
        {
            var reservedAlias = new ReservedAlias { Name = "Api" };

            repository.Add(reservedAlias);

            repository.Delete(reservedAlias);

            Assert.False(Database.ReservedAliases.Any(b => b.Id == reservedAlias.Id));
        }

        [Theory]
        [InlineData("Api","api", true, false)]
        [InlineData("Api", "api", false, true)]
        [InlineData("Api", "Api", true, true)]
        [InlineData("Api", "efapi", false, false)]
        [InlineData("Api", "ApiEf", false, true)]
        public void IsMatching_should_return_correct_value(string reservedAlias, string alias, bool expectedResult, bool useCaseSensitive)
        {
            var queryFactory = new QueryFactory(useCaseSensitive);
            
            var repo = new ReservedAliasRepository(DatabaseFactory, queryFactory);

            repo.Add(new ReservedAlias { Name = reservedAlias });

            bool ismatched = repo.IsMatching(alias);

            Assert.Equal(expectedResult,ismatched);
        }
    }
}
