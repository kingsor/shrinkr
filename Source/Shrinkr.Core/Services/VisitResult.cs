namespace Shrinkr.Services
{
    using System.Collections.Generic;

    using DataTransferObjects;

    public class VisitResult : ServiceResultBase
    {
        public VisitResult() : this(new List<RuleViolation>())
        {
        }

        public VisitResult(VisitDTO visit) : this()
        {
            Check.Argument.IsNotNull(visit, "visit");

            Visit = visit;
        }

        public VisitResult(IEnumerable<RuleViolation> ruleViolations) : base(ruleViolations)
        {
        }

        public VisitDTO Visit
        {
            get;
            private set;
        }
    }
}