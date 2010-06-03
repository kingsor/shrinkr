namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class VisitCountByAliasNameQuery : QueryBase<int>
    {
        private static readonly Expression<Func<Database, string, AliasVisitsCount>> visitsCountExpression = (database, name) => database.Aliases.Where(a => a.Name == name).Select(a => new AliasVisitsCount(a.Name, a.Visits.Count)).FirstOrDefault();
        private static readonly Func<Database, string, AliasVisitsCount> visitsCountPlainQuery = visitsCountExpression.Compile();
        
        private readonly bool caseSensitive;
        private readonly string alias;

        public VisitCountByAliasNameQuery(bool caseSensitive, string alias)
        {
            Check.Argument.IsNotNullOrEmpty(alias, "alias");

            this.caseSensitive = caseSensitive;
            this.alias = alias;
        }

        public override int Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            AliasVisitsCount result = visitsCountPlainQuery(database, alias);

            return caseSensitive
                       ? (result != null && result.Alias.Equals(alias)
                              ? result.VisitsCount
                              : 0)
                       : (result != null
                              ? result.VisitsCount
                              : 0);
        }

        private class AliasVisitsCount
        {
            public AliasVisitsCount(string alias, int visitsCount)
            {
                Alias = alias;
                VisitsCount = visitsCount;
            }

            public string Alias
            {
                get;
                private set;
            }

            public int VisitsCount
            {
                get;
                private set;
            }
        }
    }
}