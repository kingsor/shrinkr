namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;
    
    using DomainObjects;

    using Xunit;

    public class BadWordRepositoryTests : DatabaseTestBase
    {
        private readonly BadWordRepository repository;

        public BadWordRepositoryTests()
        {
            repository = new BadWordRepository(DatabaseFactory);
        }

        [Fact]
        public void Should_be_able_to_add_bad_word()
        {
            var badWord = new BadWord {Expression = "F**k"};

            repository.Add(badWord);
            
            Assert.NotEqual(0, badWord.Id);
        }

        [Fact]
        public void Should_be_able_to_delete_bad_word()
        {
            var badWord = new BadWord { Expression = "F**k" };

            repository.Add(badWord);
            
            repository.Delete(badWord);
            
            Assert.False(Database.BadWords.Any(b => b.Id == badWord.Id));
        }
    }
}
