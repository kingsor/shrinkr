namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;
    
    using DomainObjects;

    using Xunit;

    public class BannedIPAddressRepositoryTests : DatabaseTestBase
    {
        private readonly BannedIPAddressRepository repository;

        public BannedIPAddressRepositoryTests()
        {
            repository = new BannedIPAddressRepository(DatabaseFactory, new QueryFactory(false));
        }

        [Fact]
        public void Add_should_be_able_to_add_banned_ip()
        {
            var ip = new BannedIPAddress {IPAddress = "127.0.0.1"};

            repository.Add(ip);

            Assert.True(ip.Id >= 0);
        }

        [Fact]
        public void Delete_should_be_able_to_delete_banned_ip()
        {
            var ip = new BannedIPAddress { IPAddress = "127.0.0.1" };

            repository.Add(ip);

            repository.Delete(ip);

            Assert.False(Database.BannedIPAddresses.Any(b => b.Id == ip.Id));
        }

        [Fact]
        public void IsMatching_should_return_correct_value()
        {
            var ip = new BannedIPAddress { IPAddress = "127.0.0.1" };

            repository.Add(ip);

            bool ismatched = repository.IsMatching(ip.IPAddress);

            Assert.True(ismatched);
        }
    }
}
