namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class BadWordMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, e) => database.BadWords.Any(word => word.Expression == e);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();

        private readonly string badWord;

        public BadWordMatchingQuery(string badWord)
        {
            Check.Argument.IsNotNullOrEmpty(badWord, "badWord");

            this.badWord = badWord;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return plainQuery(database, badWord);
        }
    }
}
