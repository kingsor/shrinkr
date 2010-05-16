﻿namespace Shrinkr.Infrastructure.NHibernate.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class ReservedAliasMatchingQuery : QueryBase<bool>
    {
        private static readonly Expression<Func<Database, string, ReservedAlias>> reservedAliasExpression = (database, a) => database.ReservedAliases.Where(reserved => reserved.Name.Equals(a, StringComparison.Ordinal)).FirstOrDefault();
        private static readonly Func<Database, string, ReservedAlias> reservedAliasPlainQuery = reservedAliasExpression.Compile();
        
        private readonly bool caseSensitive;
        private readonly string aliasName;

        public ReservedAliasMatchingQuery(bool caseSensitive, string aliasName)
        {
            Check.Argument.IsNotNullOrEmpty(aliasName, "aliasName");

            this.caseSensitive = caseSensitive;
            this.aliasName = aliasName;
        }

        public override bool Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");
            var x = database.Aliases.Where(a => a.Name == aliasName).Select(a => new { a.Name, VisitsCount = a.Visits.Count }).ToList();
            ReservedAlias alias = reservedAliasPlainQuery(database, aliasName);

            return caseSensitive
                       ? alias != null && alias.Name.Equals(aliasName)
                       : alias != null;
        }
    }
}