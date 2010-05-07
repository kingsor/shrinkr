namespace Shrinkr.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using DomainObjects;
    using Extensions;
    using Repositories;

    public class TextMatchingSpamDetector : ISpamDetector
    {
        public static readonly string CacheKey = typeof(TextMatchingSpamDetector).FullName;

        private readonly IExternalContentService externalContentService;
        private readonly IBadWordRepository badWordRepository;
        private readonly ICacheManager cacheManager;

        public TextMatchingSpamDetector(IExternalContentService externalContentService, IBadWordRepository badWordRepository, ICacheManager cacheManager)
        {
            Check.Argument.IsNotNull(externalContentService, "externalContentService");
            Check.Argument.IsNotNull(badWordRepository, "badWordRepository");
            Check.Argument.IsNotNull(cacheManager, "cacheManager");

            this.externalContentService = externalContentService;
            this.badWordRepository = badWordRepository;
            this.cacheManager = cacheManager;
        }

        public SpamStatus CheckStatus(ShortUrl shortUrl)
        {
            Check.Argument.IsNotNull(shortUrl, "url");

            ExternalContent externalContent = null;

            try
            {
                externalContent = externalContentService.Retrieve(shortUrl.Url);
            }
            catch (WebException)
            {
            }

            if (externalContent == null)
            {
                return SpamStatus.None;
            }

            if (string.IsNullOrEmpty(externalContent.Content))
            {
                return SpamStatus.None;
            }

            string title = externalContent.Title;
            string plainContent = externalContent.Content.StripHtml().Trim();

            if (!string.IsNullOrWhiteSpace(title) && GetBadWordExpressions().Any(expression => expression.IsMatch(title)))
            {
                return SpamStatus.BadWord;
            }

            if (!string.IsNullOrWhiteSpace(plainContent) && GetBadWordExpressions().Any(expression => expression.IsMatch(plainContent)))
            {
                return SpamStatus.BadWord;
            }

            return SpamStatus.Clean;
        }

        private IEnumerable<Regex> GetBadWordExpressions()
        {
            return cacheManager.GetOrCreate(CacheKey, GetBadWordsAndConvertToExpression);
        }

        private IEnumerable<Regex> GetBadWordsAndConvertToExpression()
        {
            const RegexOptions Option = RegexOptions.IgnoreCase |
                                        RegexOptions.Singleline |
                                        RegexOptions.Multiline |
                                        RegexOptions.CultureInvariant |
                                        RegexOptions.Compiled;

            return badWordRepository.All()
                                    .ToList()
                                    .Select(badWord => new Regex(badWord.Expression, Option))
                                    .ToList();
        }
    }
}