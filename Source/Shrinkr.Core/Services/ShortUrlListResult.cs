namespace Shrinkr.Services
{
    using System.Collections.Generic;

    using DataTransferObjects;

    public class ShortUrlListResult : ServiceResultBase
    {
        public ShortUrlListResult() : this(new List<RuleViolation>())
        {
        }

        public ShortUrlListResult(PagedResult<ShortUrlDTO> pagedResult) : this()
        {
            Check.Argument.IsNotNull(pagedResult, "pagedResult");

            ShortUrls = new List<ShortUrlDTO>(pagedResult.Result);
            Total = pagedResult.Total;
        }

        public ShortUrlListResult(IEnumerable<RuleViolation> ruleViolations) : base(ruleViolations)
        {
        }

        public IList<ShortUrlDTO> ShortUrls
        {
            get;
            private set;
        }

        public int Total
        {
            get;
            private set;
        }
    }
}