namespace Shrinkr.Web
{
    using System;
    using System.Web.Mvc;

    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
    using DotNetOpenAuth.OpenId.RelyingParty;

    using Extensions;
    using Services;

    public class AuthenticationController : Controller
    {
        internal const string ModelStateUserNameKey = "userName";

        internal const string CookieRememberMe = "oidr";
        internal const string CookieReturnUrl = "oidu";

        private const int SevenDays = 60 * 60 * 24 * 7;

        private readonly IOpenIdRelyingParty openId;
        private readonly IFormsAuthentication formsAuthentication;
        private readonly ICookie cookie;
        private readonly IUserService userService;

        public AuthenticationController(IOpenIdRelyingParty openId, IFormsAuthentication formsAuthentication, ICookie cookie, IUserService userService)
        {
            Check.Argument.IsNotNull(openId, "openId");
            Check.Argument.IsNotNull(formsAuthentication, "formsAuthentication");
            Check.Argument.IsNotNull(cookie, "cookie");
            Check.Argument.IsNotNull(userService, "userService");

            this.openId = openId;
            this.formsAuthentication = formsAuthentication;
            this.cookie = cookie;
            this.userService = userService;
        }

        [OutputCache(Duration = SevenDays, VaryByParam = "none")]
        public ActionResult Xrds()
        {
            const string Xrds = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<xrds:XRDS xmlns:xrds=\"xri://$xrds\" xmlns:openid=\"http://openid.net/xmlns/1.0\" xmlns=\"xri://$xrd*($v*2.0)\">" +
                                    "<XRD>" +
                                        "<Service priority=\"1\">" +
                                            "<Type>http://specs.openid.net/auth/2.0/return_to</Type>" +
                                            "<URI>{0}</URI>" +
                                        "</Service>" +
                                    "</XRD>" +
                                "</xrds:XRDS>";

            string url = Url.ToAbsolute(Url.LogOn());

            string xml = Xrds.FormatWith(url);

            return Content(xml, "application/xrds+xml");
        }

        [AcceptVerbs(HttpVerbs.Get), AppendOpenIdXrdsLocation]
        public ActionResult LogOn()
        {
            IAuthenticationResponse response = openId.Response;

            if (response != null)
            {
                if ((response.Status == AuthenticationStatus.Failed) || (response.Status == AuthenticationStatus.Canceled))
                {
                    ModelState.AddModelError(ModelStateUserNameKey, TextMessages.UnableToLoginWithYourPreferredOpenIDProvider);
                }
                else if (response.Status == AuthenticationStatus.Authenticated)
                {
                    string userName = response.ClaimedIdentifier;
                    ClaimsResponse fetch = response.GetExtension<ClaimsResponse>();

                    // Some of the Provider does not return Email
                    // Such as Yahoo, Blogger, Bloglines etc, in that case email will be null
                    string email = (fetch != null) ? fetch.Email : null;

                    UserResult result = userService.Save(userName, email);
                    ModelState.Merge(result.RuleViolations);

                    if (ModelState.IsValid)
                    {
                        bool persistCookie = cookie.GetValue<bool>(CookieRememberMe);

                        formsAuthentication.SetAuthenticationCookie(userName, persistCookie);

                        string returnUrl = cookie.GetValue<string>(CookieReturnUrl);

                        return Redirect(returnUrl ?? Url.Home());
                    }
                }
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(LogOnCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            Identifier id;

            if (string.IsNullOrWhiteSpace(command.UserName) || !Identifier.TryParse(command.UserName, out id))
            {
                string errorMessage = string.IsNullOrWhiteSpace(command.UserName) ? TextMessages.OpenIDUserNameCannotBeBlank : TextMessages.InvalidOpenIDUserName;

                ModelState.AddModelError(ModelStateUserNameKey, errorMessage);
            }
            else
            {
                cookie.SetValue(CookieRememberMe, command.RememberMe ?? false);
                cookie.SetValue(CookieReturnUrl, !string.IsNullOrWhiteSpace(command.ReturnUrl) ? command.ReturnUrl : Url.Home());

                try
                {
                    Realm realm = new Realm(new Uri(Url.ToAbsolute(Url.LogOn())));
                    IAuthenticationRequest request = openId.CreateRequest(id, realm);

                    ClaimsRequest fetch = new ClaimsRequest { Email = DemandLevel.Request };
                    request.AddExtension(fetch);

                    request.RedirectToProvider();
                }
                catch (ProtocolException e)
                {
                    ModelState.AddModelError(ModelStateUserNameKey, e.Message);
                }
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOff()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOff(string returnUrl)
        {
            formsAuthentication.LogOff();

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Home() : returnUrl);
        }
    }
}