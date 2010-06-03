namespace Shrinkr.Services
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class ServiceResultBase
    {
        [DebuggerStepThrough]
        protected ServiceResultBase(IEnumerable<RuleViolation> ruleViolations)
        {
            Check.Argument.IsNotNull(ruleViolations, "ruleViolations");

            RuleViolations = new List<RuleViolation>(ruleViolations);
        }

        public IList<RuleViolation> RuleViolations
        {
            get;
            private set;
        }
    }
}