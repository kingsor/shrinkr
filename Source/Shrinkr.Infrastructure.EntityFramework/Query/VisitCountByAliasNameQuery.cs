namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class VisitCountByAliasNameQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, string, int>> caseInsensitiveExpression = (database, name) => database.Visits.Count(v => v.Alias.Name == name);
        private static readonly Func<Database, string, int> caseInsensitivePlainQuery = caseInsensitiveExpression.Compile();
        private static readonly Func<Database, string, int> caseInsensitiveCompiledQuery = CompiledQuery.Compile(caseInsensitiveExpression);

        private static readonly Expression<Func<Database, string, int>> caseSensitiveExpression = (database, name) => database.Visits.Count(v => v.Alias.Name.Equals(name, StringComparison.Ordinal));
        private static readonly Func<Database, string, int> caseSensitivePlainQuery = caseSensitiveExpression.Compile();
        private static readonly Func<Database, string, int> caseSensitiveCompiledQuery = CompiledQuery.Compile(caseSensitiveExpression);

        private readonly bool caseSensitive;
        private readonly string alias;

        public VisitCountByAliasNameQuery(bool caseSensitive, bool useCompiled, string alias) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            this.caseSensitive = caseSensitive;
            this.alias = alias;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled
                       ? (caseSensitive
                              ? caseSensitiveCompiledQuery(database, alias)
                              : caseInsensitiveCompiledQuery(database, alias))
                       : (caseSensitive
                              ? caseSensitivePlainQuery(database, alias)
                              : caseInsensitivePlainQuery(database, alias));
        }
    }
}