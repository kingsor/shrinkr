namespace Shrinkr.Infrastructure.EntityFramework.IntegrationTests
{
    using System;

    using DomainObjects;

    public static class ObjectMother
    {
        public static User CreateUser()
        {
            return new User { Name = Random(), Email = Random() };
        }

        public static ShortUrl CreateShortUrl()
        {
            return new ShortUrl { Title = Random(), Url = Random() };
        }

        public static Alias CreateAlias()
        {
            return new Alias { Name = Random(), IPAddress = "192.168.0.1" };
        }

        public static Alias CreateAliasWithParent()
        {
            var alias = CreateAlias();

            alias.ShortUrl = CreateShortUrl();
            alias.User = CreateUser();

            return alias;
        }

        public static Visit CreateVisit()
        {
            return new Visit { IPAddress = "192.168.0.1", Referrer = new Referrer { Url = Random(), Domain = Random() } };
        }

        public static Visit CreateVisitWithParent()
        {
            var visit = CreateVisit();

            visit.Alias = CreateAliasWithParent();

            return visit;
        }

        private static string Random()
        {
            return Guid.NewGuid().ToString();
        }
    }
}