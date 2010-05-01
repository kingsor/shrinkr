namespace Shrinkr.Services
{
    using System.Collections.Generic;

    using DomainObjects;

    public class AdministrativeActionResult<TItem> : ServiceResultBase where TItem : IEntity
    {
        public AdministrativeActionResult() : this(new List<RuleViolation>())
        {
        }

        public AdministrativeActionResult(TItem item) : this()
        {
            Check.Argument.IsNotNull(item, "item");

            Item = item;
        }

        public AdministrativeActionResult(IEnumerable<RuleViolation> ruleViolations) : base(ruleViolations)
        {
        }

        public TItem Item
        {
            get;
            private set;
        }
    }
}