namespace Shrinkr.Web
{
    using System.Web.Mvc;

    using Services;

    public class ApiController : Controller
    {
        private readonly IShortUrlService shortUrlService;

        public ApiController(IShortUrlService shortUrlService)
        {
            Check.Argument.IsNotNull(shortUrlService, "shortUrlService");

            this.shortUrlService = shortUrlService;
        }

        public ActionResult Create(CreateShortUrlApiCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            ShortUrlResult result = shortUrlService.CreateWithApiKey(command.Url, command.Alias, command.IPAddress, command.ApiKey);

            ModelState.Merge(result.RuleViolations);

            if (result.ShortUrl != null)
            {
                ViewData.Model = new CreateUrlViewModel(result.ShortUrl);
            }

            return new ApiResult(command.ResponseFormat);
        }
    }
}