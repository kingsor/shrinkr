namespace Shrinkr.Infrastructure.NHibernate.IntegrationTests
{
    using System;

    using Extensions;
    using DomainObjects;

    public static class ObjectMother
    {
        public static User CreateUser()
        {
            return new User { Name = Random(), Email = Random() };
        }

        public static ShortUrl CreateShortUrl()
        {
            string url = Random();
            return new ShortUrl
            {
                Domain = Random(),
                Url = url,
                Title = "Some title",
                Hash = url.Hash(),
            };
        }

        public static Alias CreateAlias()
        {
            return new Alias
            {
                Name = Random(),
                ShortUrl = CreateShortUrl(),
                User = CreateUser(),
                IPAddress = "192.168.0.1",
                CreatedAt = SystemTime.Now()
            };
        }

        public static Visit CreateVisit()
        {
            return new Visit
            {
                Alias = CreateAlias(),
                CreatedAt = SystemTime.Now(),
                IPAddress = "192.168.0.1",
                Referrer = new Referrer { Url = Random(), Domain = Random() }
            };
        }

        private static string Random()
        {
            return Guid.NewGuid().ToString();
        }
    }
}