namespace Shrinkr.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Data.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class UserByApiKeyQuery : QueryBase<User>
    {
        private static readonly Expression<Func<Database, string, User>> expression = (database, key) => database.Users.SingleOrDefault(user => user.ApiSetting.Key == key);
        private static readonly Func<Database, string, User> plainQuery = expression.Compile();
        private static readonly Func<Database, string, User> compiledQuery = CompiledQuery.Compile(expression);

        private readonly string apiKey;

        public UserByApiKeyQuery(bool useCompiled, string apiKey) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(apiKey, "apiKey");

            this.apiKey = apiKey;
        }

        public override User Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   compiledQuery(database, apiKey) :
                   plainQuery(database, apiKey);
        }
    }
}