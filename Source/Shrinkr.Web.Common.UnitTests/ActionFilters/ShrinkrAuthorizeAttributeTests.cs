namespace Shrinkr.Web.UnitTests
{
    using System.Security.Principal;
    using System.Web.Mvc;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Moq;
    using Xunit;

    public class ShrinkrAuthorizeAttributeTests
    {
        private readonly Mock<IUserService> userService;
        private readonly ShrinkrAuthorizeAttribute attribute;

        public ShrinkrAuthorizeAttributeTests()
        {
            userService = new Mock<IUserService>();
            attribute = new ShrinkrAuthorizeAttribute(userService.Object);
        }

        [Fact]
        public void IsAuthorized_should_return_true_when_current_user_is_authenticated_and_not_locked()
        {
            var identity = new Mock<IIdentity>();
            var httpContext = MvcTestHelper.CreateHttpContext();

            var user = new Mock<User>();
            user.Setup(u => u.IsLockedOut).Returns(false);
            user.Setup(u => u.ApiSetting).Returns(new ApiSetting());

            userService.Setup(srv => srv.GetByName(It.IsAny<string>())).Returns(new UserDTO(user.Object));

            identity.Setup(i => i.IsAuthenticated).Returns(true);
            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            Assert.True(attribute.IsAuthorized(new AuthorizationContext { HttpContext = httpContext.Object }));
        }

        [Fact]
        public void IsAuthorized_should_return_false_when_current_user_is_authenticated_but_locked()
        {
            var identity = new Mock<IIdentity>();
            var httpContext = MvcTestHelper.CreateHttpContext();

            var user = new Mock<User>();
            user.Setup(u => u.IsLockedOut).Returns(true);
            user.Setup(u => u.ApiSetting).Returns(new ApiSetting());

            userService.Setup(srv => srv.GetByName(It.IsAny<string>())).Returns(new UserDTO(user.Object));

            identity.Setup(i => i.IsAuthenticated).Returns(true);
            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            Assert.False(attribute.IsAuthorized(new AuthorizationContext { HttpContext = httpContext.Object }));
        }

        [Fact]
        public void IsAuthorized_should_return_false_when_current_user_is_not_authenticated()
        {
            var identity = new Mock<IIdentity>();
            var httpContext = MvcTestHelper.CreateHttpContext();

            identity.Setup(i => i.IsAuthenticated).Returns(false);
            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            Assert.False(attribute.IsAuthorized(new AuthorizationContext { HttpContext = httpContext.Object }));
        }

        [Fact]
        public void IsAuthorized_should_return_true_when_current_user_is_authenticated_has_allowed_role()
        {
            var user = new UserDTO(new User { Role = Role.Administrator });
            var identity = new Mock<IIdentity>();
            var httpContext = MvcTestHelper.CreateHttpContext();

            identity.Setup(i => i.IsAuthenticated).Returns(true);
            identity.Setup(i => i.Name).Returns("FakeUser");

            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            userService.Setup(r => r.GetByName("FakeUser")).Returns(user);

            attribute.AllowedRole = Role.Administrator;

            Assert.True(attribute.IsAuthorized(new AuthorizationContext { HttpContext = httpContext.Object }));
        }

        [Fact]
        public void IsAuthorized_should_return_false_when_current_user_is_authenticated_has_no_allowed_role()
        {
            var user = new UserDTO(new User { Role = Role.User });
            var identity = new Mock<IIdentity>();
            var httpContext = MvcTestHelper.CreateHttpContext();

            identity.Setup(i => i.IsAuthenticated).Returns(true);
            identity.Setup(i => i.Name).Returns("FakeUser");

            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            userService.Setup(r => r.GetByName("FakeUser")).Returns(user);

            attribute.AllowedRole = Role.Administrator;

            Assert.False(attribute.IsAuthorized(new AuthorizationContext { HttpContext = httpContext.Object }));
        }
    }
}