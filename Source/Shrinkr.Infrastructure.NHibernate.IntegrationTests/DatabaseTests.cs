namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System.Linq;

    using Xunit;

    public class DatabaseTests : DatabaseTestBase
    {
        [Fact]
        public void Should_be_able_to_get_users()
        {
            int count;

            Assert.DoesNotThrow(() => { count = Database.Users.Count(); });
        }
    }
}