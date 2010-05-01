namespace Shrinkr.Services
{
    using System.Collections.Generic;

    using DataTransferObjects;

    public class UserResult : ServiceResultBase
    {
        public UserResult() : this(new List<RuleViolation>())
        {
        }

        public UserResult(UserDTO user) : this()
        {
            Check.Argument.IsNotNull(user, "user");

            User = user;
        }

        public UserResult(IEnumerable<RuleViolation> ruleViolations) : base(ruleViolations)
        {
        }

        public UserDTO User
        {
            get;
            private set;
        }
    }
}