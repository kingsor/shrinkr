namespace Shrinkr.Web
{
    using System.Web.Mvc;

    using MvcExtensions;

    using DataTransferObjects;
    using DomainObjects;
    using Infrastructure;
    using Services;

    public class ShortUrlController : Controller
    {
        private readonly IShortUrlService shortUrlService;
        private readonly Settings settings;

        public ShortUrlController(IShortUrlService shortUrlService, Settings settings)
        {
            Check.Argument.IsNotNull(shortUrlService, "shortUrlService");
            Check.Argument.IsNotNull(settings, "settings");

            this.shortUrlService = shortUrlService;
            this.settings = settings;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportViewDataFromTempData]
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken, ExportViewDataToTempDataAttribute]
        public ActionResult Create(CreateShortUrlCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            ShortUrlResult result = shortUrlService.CreateWithUserName(command.Url, command.Alias, command.IPAddress, command.UserName);

            object model = (result.ShortUrl != null) ? new CreateUrlViewModel(result.ShortUrl) : null;

            return this.AdaptivePostRedirectGet(result.RuleViolations, model, Url.Create());
        }

        public ActionResult Preview(ShortUrlVisitCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            ShortUrlResult result = shortUrlService.GetByAlias(command.Alias);

            ModelState.Merge(result.RuleViolations);

            if (result.ShortUrl == null)
            {
                return new NotFoundResult();
            }

            return View(result.ShortUrl);
        }

        public ActionResult Visit(ShortUrlVisitCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            bool sameDomain = !string.IsNullOrWhiteSpace(command.Referrer) && command.Referrer.StartsWith(Url.ApplicationRoot());

            if (!sameDomain)
            {
                ShortUrlResult shortUrlResult = shortUrlService.GetByAlias(command.Alias);
                ShortUrlDTO shortUrl = shortUrlResult.ShortUrl;

                if (shortUrl == null)
                {
                    return new NotFoundResult();
                }

                if (shortUrl.SpamStatus.IsSpam())
                {
                    return RedirectToAction("Preview", new { alias = shortUrl.Alias });
                }
            }

            if (sameDomain)
            {
                command.Referrer = null;
            }

            VisitResult visitResult = shortUrlService.Visit(command.Alias, command.IPAddress, command.Browser, command.Referrer);

            ModelState.Merge(visitResult.RuleViolations);

            if (visitResult.Visit == null)
            {
                return new NotFoundResult();
            }

            string url = visitResult.Visit.Url;

            return settings.RedirectPermanently ? new PermanentRedirectResult(url) : Redirect(url);
        }

        public ActionResult List(ShortUrlListCommand command)
        {
            int urlPerPage = settings.UrlPerPage;
            ShortUrlListResult result = shortUrlService.FindByUser(command.UserName, PageCalculator.StartIndex(command.Page, urlPerPage), urlPerPage);

            object model = new PagedListViewModel<ShortUrlDTO>(result.ShortUrls, command.Page ?? 1, urlPerPage, result.Total);

            return this.AdaptiveView(result.RuleViolations, model);
        }
    }
}