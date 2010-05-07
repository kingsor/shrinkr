namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Web.Mvc;

    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.RelyingParty;
    using DotNetOpenAuth.Messaging;

    using Services;

    using Moq;
    using Xunit;
    using Xunit.Extensions;

    public class AuthenticationControllerTests
    {
        private readonly Mock<IOpenIdRelyingParty> openIdRelayingParty;
        private readonly Mock<IFormsAuthentication> formsAuth;
        private readonly Mock<ICookie> cookie;
        private readonly Mock<IUserService> userService;

        private readonly AuthenticationController controller;

        public AuthenticationControllerTests()
        {
            new RegisterRoutes().Execute();

            openIdRelayingParty = new Mock<IOpenIdRelyingParty>();
            formsAuth = new Mock<IFormsAuthentication>();
            cookie = new Mock<ICookie>();
            userService = new Mock<IUserService>();

            controller = new AuthenticationController(openIdRelayingParty.Object, formsAuth.Object, cookie.Object, userService.Object);
        }

        [Fact]
        public void Xrds_should_return_xml_result()
        {
            var httpContext = controller.MockHttpContext();

            httpContext.SetupGet(c => c.Request.Url).Returns(new Uri(MvcTestHelper.AppPathModifier));
            httpContext.SetupGet(c => c.Request.ApplicationPath).Returns("/");

            ContentResult result = (ContentResult) controller.Xrds();

            Assert.Equal("application/xrds+xml", result.ContentType);
            Assert.False(string.IsNullOrWhiteSpace(result.Content));
        }

        [Theory]
        [InlineData(AuthenticationStatus.Failed)]
        [InlineData(AuthenticationStatus.Canceled)]
        public void LogOn_get_should_add_error_to_model_state_when_authentication_failed_or_cancelled(AuthenticationStatus authStatus)
        {
            var authResponse = new Mock<IAuthenticationResponse>();
            
            openIdRelayingParty.Setup(o => o.Response).Returns(authResponse.Object);
            authResponse.Setup(a => a.Status).Returns(authStatus);
            
            controller.LogOn();
            
            string actualErrMsg = controller.ModelState[AuthenticationController.ModelStateUserNameKey].Errors[0].ErrorMessage;

            Assert.Equal("Unable to login with your preferred OpenID provider.", actualErrMsg);
        }

        [Fact]
        public void LogOn_get_should_set_authentication_cookie_when_authenticated_and_logon_is_valid()
        {
            controller.MockHttpContext("/", "~/LogOn", "Get");

            var authResponse = new Mock<IAuthenticationResponse>();
            openIdRelayingParty.Setup(o => o.Response).Returns(authResponse.Object);
            authResponse.Setup(a => a.Status).Returns(AuthenticationStatus.Authenticated);
            userService.Setup(svc => svc.Save(It.IsAny<string>(), It.IsAny<string>())).Returns(new UserResult());
            
            controller.LogOn();

            formsAuth.Verify(f => f.SetAuthenticationCookie(It.IsAny<string>(), It.IsAny<bool>()));
        }

        [Fact]
        public void LogOn_get_should_redirect_to_home_if_no_return_url_available_when_user_is_auhtenticated_and_logon_is_valid()
        {
            controller.MockHttpContext("/", "~/LogOn", "Get");

            var authResponse = new Mock<IAuthenticationResponse>();
            openIdRelayingParty.Setup(o => o.Response).Returns(authResponse.Object);
            authResponse.Setup(a => a.Status).Returns(AuthenticationStatus.Authenticated);
            userService.Setup(svc => svc.Save(It.IsAny<string>(), It.IsAny<string>())).Returns(new UserResult());
            cookie.Setup(c => c.GetValue<string>(AuthenticationController.CookieReturnUrl)).Returns((string)null);
            
            var actionResult = (RedirectResult)controller.LogOn();

            Assert.Equal(controller.Url.Home(), actionResult.Url);
        }

        [Fact]
        public void LogOn_get_should_redirect_to_return_url_when_available_and_logon_is_valid()
        {
            controller.MockHttpContext("/", "~/LogOn", "Get");

            var authResponse = new Mock<IAuthenticationResponse>();
            openIdRelayingParty.Setup(o => o.Response).Returns(authResponse.Object);
            authResponse.Setup(a => a.Status).Returns(AuthenticationStatus.Authenticated);
            userService.Setup(svc => svc.Save(It.IsAny<string>(), It.IsAny<string>())).Returns(new UserResult());
            cookie.Setup(c => c.GetValue<string>(AuthenticationController.CookieReturnUrl)).Returns("~/Preview");

            var actionResult = (RedirectResult)controller.LogOn();

            Assert.Equal("~/Preview", actionResult.Url);
        }

        [Fact]
        public void LogOn_get_should_not_set_authentication_cookie_when_authenticated_but_logon_is_not_valid()
        {
            controller.MockHttpContext("/", "~/LogOn", "Get");

            var authResponse = new Mock<IAuthenticationResponse>();
            openIdRelayingParty.Setup(o => o.Response).Returns(authResponse.Object);
            authResponse.Setup(a => a.Status).Returns(AuthenticationStatus.Authenticated);

            var userResult = new UserResult();
            userResult.RuleViolations.Add(new RuleViolation("param", "some error message"));
            userService.Setup(svc => svc.Save(It.IsAny<string>(), It.IsAny<string>())).Returns(userResult);

            formsAuth.Verify(f => f.SetAuthenticationCookie(It.IsAny<string>(), It.IsAny<bool>()), Times.Never());
        }

        [Theory]
        [InlineData(null, "OpenID user name cannot be blank.")]
        [InlineData("", "OpenID user name cannot be blank.")]
        [InlineData("~!@#$*", "Invalid OpenID user name.")]
        public void LogOn_post_should_add_error_to_model_state_when_user_name_is_invalid(string username, string expectedErrorMsg)
        {
            controller.LogOn(new LogOnCommand { UserName = username, RememberMe = false, ReturnUrl = "/Preview" });

            string actualErrMsg = controller.ModelState[AuthenticationController.ModelStateUserNameKey].Errors[0].ErrorMessage;

            Assert.Equal(expectedErrorMsg, actualErrMsg);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LogOn_post_should_set_remember_me_in_cookie(bool rememberMe)
        {
            var authRequest = new Mock<IAuthenticationRequest>();
            controller.MockHttpContext("/", "~/LogOn", "Post");

            openIdRelayingParty.Setup(o => o.CreateRequest(It.IsAny<Identifier>(), It.IsAny<Realm>())).Returns(authRequest.Object);

            controller.LogOn(new LogOnCommand { UserName = "http://kazimanzurrashid.myopenid.com/", RememberMe = rememberMe, ReturnUrl = "/Preview" });

            cookie.Verify(c => c.SetValue(AuthenticationController.CookieRememberMe, rememberMe));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("/Preview")]
        public void LogOn_post_should_set_return_url_in_cookie(string returnUrl)
        {
            var authRequest = new Mock<IAuthenticationRequest>();
            controller.MockHttpContext("/", "~/LogOn", "Post");

            openIdRelayingParty.Setup(o => o.CreateRequest(It.IsAny<Identifier>(), It.IsAny<Realm>()))
                               .Returns(authRequest.Object);

            controller.LogOn(new LogOnCommand { UserName = "http://kazimanzurrashid.myopenid.com/", RememberMe = false, ReturnUrl = returnUrl });

            string expectedUrl = string.IsNullOrEmpty(returnUrl) ? controller.Url.Home() : returnUrl;

            cookie.Verify(c => c.SetValue(AuthenticationController.CookieReturnUrl, expectedUrl));
        }

        [Fact]
        public void LogOn_post_should_redirect_to_open_id_provider()
        {
            var authRequest = new Mock<IAuthenticationRequest>();
            controller.MockHttpContext("/", "~/LogOn", "Post");

            openIdRelayingParty.Setup(o => o.CreateRequest(It.IsAny<Identifier>(), It.IsAny<Realm>())).Returns(authRequest.Object);

            controller.LogOn(new LogOnCommand { UserName = "http://kazimanzurrashid.myopenid.com/", RememberMe = false });

            authRequest.Verify(r => r.RedirectToProvider());
        }

        [Fact]
        public void LogOn_post_should_add_openId_exception_message_in_modelState_when_exception_occurrs()
        {
            var authRequest = new Mock<IAuthenticationRequest>();
            controller.MockHttpContext("/", "~/LogOn", "Post");

            openIdRelayingParty.Setup(o => o.CreateRequest(It.IsAny<Identifier>(), It.IsAny<Realm>())).Returns(authRequest.Object);

            var exception = new ProtocolException("Exception Message");
            authRequest.Setup(r => r.RedirectToProvider()).Throws(exception);

            controller.LogOn(new LogOnCommand { UserName = "http://kazimanzurrashid.myopenid.com/", RememberMe = false });

            Assert.Equal(exception.Message, controller.ModelState[AuthenticationController.ModelStateUserNameKey].Errors[0].ErrorMessage);
        }

        [Fact]
        public void LogOff_post_should_log_off()
        {
            controller.MockHttpContext("/", "~/LogOff", "post");

            controller.LogOff("/Home");

            formsAuth.Verify(f => f.LogOff());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("/Preview")]
        public void LogOff_post_should_redirect_to_return_url(string returnUrl)
        {
            controller.MockHttpContext("/", "~/LogOff", "post");

            var redirectResult = (RedirectResult)controller.LogOff(returnUrl);

            string expectedReturnUrl = string.IsNullOrEmpty(returnUrl) ? controller.Url.Home() : returnUrl;

            Assert.Equal(expectedReturnUrl, redirectResult.Url);
        }
    }
}