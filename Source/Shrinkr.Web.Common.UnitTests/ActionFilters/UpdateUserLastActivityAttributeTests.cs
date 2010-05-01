namespace Shrinkr.Web.UnitTests
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Security.Principal;

    using Services;

    using Moq;
    using Xunit;

    public class UpdateUserLastActivityAttributeTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly Mock<IIdentity> identity;
        private readonly Mock<IUserService> userService;

        private readonly UpdateUserLastActivityAttribute attribute;

        public UpdateUserLastActivityAttributeTests()
        {
            httpContext = MvcTestHelper.CreateHttpContext();
            identity = new Mock<IIdentity>();
            userService = new Mock<IUserService>();

            attribute = new UpdateUserLastActivityAttribute(userService.Object);
        }

        [Fact]
        public void OnResultExecuted_should_update_user_last_activity_when_user_is_authenticated()
        {
            identity.Setup(i => i.IsAuthenticated).Returns(true);
            identity.Setup(i => i.Name).Returns("FakeUser");

            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            var resultExcutedContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, null);

            attribute.OnResultExecuted(resultExcutedContext);

            userService.Verify(s => s.UpdateLastActivity("FakeUser"), Times.Once());
        }

        [Fact]
        public void OnResultExecuted_should_never_update_user_last_activity_when_user_is_not_authenticated()
        {
            identity.Setup(i => i.IsAuthenticated).Returns(false);
            httpContext.SetupGet(c => c.User.Identity).Returns(identity.Object);

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var resultExcutedContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, null);

            attribute.OnResultExecuted(resultExcutedContext);

            userService.Verify(s => s.UpdateLastActivity("FakeUser"), Times.Never());
        }
    }
}
