namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class UserByNameQuery : QueryBase<User>
    {
        private static readonly Expression<Func<Database, string, User>> expression = (database, name) => database.Users.SingleOrDefault(user => user.Name == name);
        private static readonly Func<Database, string, User> plainQuery = expression.Compile();
        private static readonly Func<Database, string, User> compiledQuery = CompiledQuery.Compile(expression);

        private readonly string userName;

        public UserByNameQuery(bool useCompiled, string userName) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");

            this.userName = userName;
        }

        public override User Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, userName) :
                   plainQuery(database, userName);
        }
    }
}