namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DataTransferObjects;
    using DomainObjects;
    using Infrastructure;
    using MvcExtensions;
    using Services;
    using Telerik.Web.Mvc;

    public class ControlPanelController : Controller
    {
        private const int ItemPerPage = 10;

        private readonly IAdministrativeService administrativeService;

        public ControlPanelController(IAdministrativeService administrativeService)
        {
            Check.Argument.IsNotNull(administrativeService, "administrativeService");

            this.administrativeService = administrativeService;
        }

        public ActionResult Summary()
        {
            IDictionary<TimeSpan, SystemHealthDTO> status = administrativeService.GetHealthStatus(SystemTime.Now());

            return View(status);
        }

        [GridAction]
        public ActionResult Urls(int? page, string orderBy, string filter)
        {
            IEnumerable<ShortUrlDTO> urls = administrativeService.GetShortUrls();

            return View(new GridModel<ShortUrlDTO> { Data = urls });
        }

        [ActionName("Url")]
        public ActionResult ShortUrl(string alias)
        {
            ShortUrlDTO model = administrativeService.GetShortUrl(alias);

            return View(model);
        }

        [HttpPost]
        public ActionResult MarkUrlAsSpam(string alias)
        {
            return UpdateShortUrlSpamStatus(alias, SpamStatus.BadWord);
        }

        [HttpPost]
        public ActionResult MarkUrlAsSafe(string alias)
        {
            return UpdateShortUrlSpamStatus(alias, SpamStatus.Clean);
        }

        [GridAction]
        public ActionResult Users(int? page, string orderBy, string filter)
        {
            IEnumerable<UserDTO> users = administrativeService.GetUsers();

            return View(new GridModel<UserDTO> { Data = users });
        }

        [ActionName("User")]
        public ActionResult Member(long id)
        {
            UserDTO model = administrativeService.GetUser(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult LockUser(long id)
        {
            return LockOrUnlockUser(id, false);
        }

        [HttpPost]
        public ActionResult UnlockUser(long id)
        {
            return LockOrUnlockUser(id, true);
        }

        [HttpPost]
        public ActionResult UpdateUserRole(long id, Role role)
        {
            administrativeService.UpdateUserRole(id, role);

            return this.AdaptivePostRedirectGet(null, administrativeService.GetUser(id), Url.User(id));
        }

        [HttpPost]
        public ActionResult UpdateUserApiAccess(long id, int dailyLimit)
        {
            administrativeService.UpdateUserApiAccess(id, dailyLimit);

            return this.AdaptivePostRedirectGet(null, administrativeService.GetUser(id), Url.User(id));
        }

        [HttpPost]
        public ActionResult ChangeUserRoleToAdministrator(long id)
        {
            return UpdateUserRole(id, Role.Administrator);
        }

        [HttpPost]
        public ActionResult ChangeUserRoleToUser(long id)
        {
            return UpdateUserRole(id, Role.User);
        }

        [ImportViewDataFromTempData]
        public ActionResult BannedIPAddresses(int? page)
        {
            return PrepareListActionResult(page, () => administrativeService.GetBannedIPAddresses());
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult CreateBannedIPAddress(string ipAddress)
        {
            AdministrativeActionResult<BannedIPAddress> result = administrativeService.CreateBannedIPAddress(ipAddress);

            return this.AdaptivePostRedirectGet(result.RuleViolations, result.Item, Url.BannedIPAddresses(1));
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult DeleteBannedIPAddress(long id)
        {
            administrativeService.DeleteBannedIPAddress(id);

            return this.AdaptivePostRedirectGet(Url.BannedIPAddresses(1));
        }

        [ImportViewDataFromTempData]
        public ActionResult BannedDomains(int page)
        {
            return PrepareListActionResult(page, () => administrativeService.GetBannedDomains());
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult CreateBannedDomain(string name)
        {
            AdministrativeActionResult<BannedDomain> result = administrativeService.CreateBannedDomain(name);

            return this.AdaptivePostRedirectGet(result.RuleViolations, result.Item, Url.BannedDomains(1));
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult DeleteBannedDomain(long id)
        {
            administrativeService.DeleteBannedDomain(id);

            return this.AdaptivePostRedirectGet(Url.BannedDomains(1));
        }

        [ImportViewDataFromTempData]
        public ActionResult ReservedAliases(int page)
        {
            return PrepareListActionResult(page, () => administrativeService.GetReservedAliases());
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult CreateReservedAlias(string aliasName)
        {
            AdministrativeActionResult<ReservedAlias> result = administrativeService.CreateReservedAlias(aliasName);

            return this.AdaptivePostRedirectGet(result.RuleViolations, result.Item, Url.ReservedAliases(1));
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult DeleteReservedAlias(long id)
        {
            administrativeService.DeleteReservedAlias(id);

            return this.AdaptivePostRedirectGet(Url.ReservedAliases(1));
        }

        [ImportViewDataFromTempData]
        public ActionResult BadWords(int page)
        {
            return PrepareListActionResult(page, () => administrativeService.GetBadWords());
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult CreateBadWord(string expression)
        {
            AdministrativeActionResult<BadWord> result = administrativeService.CreateBadWord(expression);

            return this.AdaptivePostRedirectGet(result.RuleViolations, result.Item, Url.BadWords(1));
        }

        [HttpPost, ExportViewDataToTempData]
        public ActionResult DeleteBadWord(long id)
        {
            administrativeService.DeleteBadWord(id);

            return this.AdaptivePostRedirectGet(Url.BadWords(1));
        }

        private ActionResult UpdateShortUrlSpamStatus(string alias, SpamStatus status)
        {
            administrativeService.UpdateShortUrlSpamStatus(alias, status);

            return this.AdaptivePostRedirectGet(null, administrativeService.GetShortUrl(alias), Url.Url(alias));
        }

        private ActionResult LockOrUnlockUser(long id, bool unlock)
        {
            administrativeService.LockOrUnlockUser(id, unlock);

            return this.AdaptivePostRedirectGet(null, administrativeService.GetUser(id), Url.User(id));
        }

        private ActionResult PrepareListActionResult<TItem>(int? page, Func<IEnumerable<TItem>> getItems) where TItem : class
        {
            IEnumerable<TItem> items = getItems().Skip(PageCalculator.StartIndex(page, ItemPerPage))
                                                 .Take(ItemPerPage);

            int count = getItems().Count();

            ViewData.Model = new PagedListViewModel<TItem>(items, page ?? 1, ItemPerPage, count);

            return this.AdaptiveView();
        }
    }
}