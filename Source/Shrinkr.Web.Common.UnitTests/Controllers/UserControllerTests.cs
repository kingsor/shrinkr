namespace Shrinkr.Web.UnitTests
{
    using MvcExtensions;

    using Services;

    using Moq;
    using Xunit;

    public class UserControllerTests
    {
        private readonly Mock<IUserService> userService;
        private readonly UserController controller;

        public UserControllerTests()
        {
            new RegisterRoutes().Execute();

            userService = new Mock<IUserService>();
            controller = new UserController(userService.Object);
        }

        [Fact]
        public void Profile_should_use_user_service_to_get_user_by_name()
        {
            controller.Profile(new ProfileCommand());

            userService.Verify(s => s.GetByName(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateKey_should_use_user_service_to_regenerate_api_key()
        {
            userService.Setup(s => s.RegenerateApiKey(It.IsAny<string>())).Returns(new UserResult()).Verifiable();
            controller.MockHttpContext("/", "~/User/MyProfile", "GET");
            
            controller.GenerateKey(new ProfileCommand());

            userService.VerifyAll();
        }

        [Fact]
        public void GenerateKey_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            userService.Setup(s => s.RegenerateApiKey(It.IsAny<string>())).Returns(new UserResult());
            controller.MockHttpContext("/", "~/MyProfile", "POST");

            string url = controller.Url.Profile();
            var view = (AdaptivePostRedirectGetResult)controller.GenerateKey(new ProfileCommand());

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }
    }
}
