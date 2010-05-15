namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;
    
    using DomainObjects;

    using Xunit;
    using Xunit.Extensions;

    public class BadWordRepositoryTests : DatabaseTestBase
    {
        private readonly BadWordRepository repository;

        public BadWordRepositoryTests()
        {
            repository = new BadWordRepository(DatabaseFactory, new QueryFactory(false));
        }

        [Fact]
        public void Add_should_be_able_to_add_bad_word()
        {
            var badWord = new BadWord { Expression = "F**k" };

            repository.Add(badWord);
            
            Assert.True(badWord.Id >= 0);
        }

        [Fact]
        public void Delete_should_be_able_to_delete_bad_word()
        {
            var badWord = new BadWord { Expression = "F**k" };

            repository.Add(badWord);
            
            repository.Delete(badWord);
            
            Assert.False(Database.BadWords.Any(b => b.Id == badWord.Id));
        }

        [Theory]
        [InlineData("A$$", "A$$", true)]
        [InlineData("A$$", "a$$", true)]
        [InlineData("A$$", "Ace", false)]
        [InlineData("A$$", "ace", false)]
        public void IsMatching_should_return_correct_value(string badword, string expression, bool expected)
        {
            var badWord = new BadWord { Expression = badword };
            
            repository.Add(badWord);

            bool ismatched = repository.IsMatching(expression);

            Assert.Equal(expected, ismatched);
        }
    }
}
