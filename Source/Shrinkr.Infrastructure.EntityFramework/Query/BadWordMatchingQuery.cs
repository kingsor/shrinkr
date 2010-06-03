namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class BadWordMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> expression = (database, e) => database.BadWords.Any(word => word.Expression == e);
        private static readonly Func<Database, string, bool> plainQuery = expression.Compile();
        private static readonly Func<Database, string, bool> compiledQuery = CompiledQuery.Compile(expression);

        private readonly string badWord;

        public BadWordMatchingQuery(bool useCompiled, string badWord) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(badWord, "badWord");

            this.badWord = badWord;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, badWord) :
                   plainQuery(database, badWord);
        }
    }
}