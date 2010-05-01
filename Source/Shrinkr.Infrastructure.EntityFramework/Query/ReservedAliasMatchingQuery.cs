namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    public class ReservedAliasMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, bool>> caseInsensitiveExpression = (database, a) => database.ReservedAliases.Any(reserved => reserved.Name == a);
        private static readonly Func<Database, string, bool> caseInsensitivePlainQuery = caseInsensitiveExpression.Compile();
        private static readonly Func<Database, string, bool> caseInsensitiveCompiledQuery = CompiledQuery.Compile(caseInsensitiveExpression);

        private static readonly Expression<Func<Database, string, bool>> caseSensitiveExpression = (database, a) => database.ReservedAliases.Any(reserved => reserved.Name.Equals(a));
        private static readonly Func<Database, string, bool> caseSensitivePlainQuery = caseSensitiveExpression.Compile();
        private static readonly Func<Database, string, bool> caseSensitiveCompiledQuery = CompiledQuery.Compile(caseSensitiveExpression);

        private readonly bool caseSensitive;
        private readonly string aliasName;

        public ReservedAliasMatchingQuery(bool caseSensitive, bool useCompiled, string aliasName) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(aliasName, "aliasName");

            this.caseSensitive = caseSensitive;
            this.aliasName = aliasName;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled
                       ? (caseSensitive
                              ? caseSensitiveCompiledQuery(database, aliasName)
                              : caseInsensitiveCompiledQuery(database, aliasName))
                       : (caseSensitive
                              ? caseSensitivePlainQuery(database, aliasName)
                              : caseInsensitivePlainQuery(database, aliasName));
        }
    }
}