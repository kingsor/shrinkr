namespace Shrinkr.Services
{
    using System.Collections.Generic;

    using DataTransferObjects;

    public class ShortUrlResult : ServiceResultBase
    {
        public ShortUrlResult() : this(new List<RuleViolation>())
        {
        }

        public ShortUrlResult(ShortUrlDTO shortUrl) : this()
        {
            Check.Argument.IsNotNull(shortUrl, "shortUrl");

            ShortUrl = shortUrl;
        }

        public ShortUrlResult(IEnumerable<RuleViolation> ruleViolations) : base(ruleViolations)
        {
        }

        public ShortUrlDTO ShortUrl
        {
            get;
            private set;
        }
    }
}