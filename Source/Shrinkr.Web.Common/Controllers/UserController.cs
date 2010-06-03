namespace Shrinkr.Web
{
    using System.Web.Mvc;

    using DataTransferObjects;
    using MvcExtensions;
    using Services;

    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            Check.Argument.IsNotNull(userService, "userService");

            this.userService = userService;
        }

        [ImportViewDataFromTempData]
        public ActionResult Profile(ProfileCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            UserDTO model = userService.GetByName(command.UserName);

            return View(model);
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult GenerateKey(ProfileCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            UserResult result = userService.RegenerateApiKey(command.UserName);

            object model = result.User != null ? new { apiKey = result.User.ApiKey } : null;

            return this.AdaptivePostRedirectGet(result.RuleViolations, model, Url.Profile());
        }
    }
}