namespace Shrinkr.Infrastructure.NHibernate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using DomainObjects;
    using Repositories;

    //
    public class BadWordRepository : RepositoryBase<BadWord>, IBadWordRepository
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, e) => database.BadWords.Any(word => word.Expression == e);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();

        public BadWordRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public bool IsMatching(string expressionToMatch)
        {
            return plainQuery(Database, expressionToMatch);
        }
    }
}
