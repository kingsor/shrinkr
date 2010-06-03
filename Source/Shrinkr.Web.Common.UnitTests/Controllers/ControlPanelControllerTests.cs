namespace Shrinkr.Web.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using MvcExtensions;
    using Telerik.Web.Mvc;

    using DataTransferObjects;
    using DomainObjects;
    using Services;

    using Moq;

    using Xunit;
    using Xunit.Extensions;

    public class ControlPanelControllerTests
    {
        private readonly Mock<IAdministrativeService> adminService;
        private readonly ControlPanelController controller;
        
        public ControlPanelControllerTests()
        {
            new RegisterRoutes().Execute();

            adminService = new Mock<IAdministrativeService>();
            controller = new ControlPanelController(adminService.Object);
        }

        [Fact]
        public void Summary_should_return_view_result_with_health_status_as_model()
        {
            var status = new Mock<IDictionary<TimeSpan, SystemHealthDTO>>();

            adminService.Setup(svc => svc.GetHealthStatus(It.IsAny<DateTime>())).Returns(status.Object);

            var view = (ViewResult)controller.Summary();

            Assert.Same(status.Object, view.ViewData.Model);
        }

        [Fact]
        public void Urls_should_return_view_result_with_short_urls_in_model()
        {
            var shortUrls = new Mock<IEnumerable<ShortUrlDTO>>();
            
            adminService.Setup(svc => svc.GetShortUrls()).Returns(shortUrls.Object);

            var view = (ViewResult)controller.Urls(null, null, null);

            Assert.Same(shortUrls.Object, ((GridModel<ShortUrlDTO>)view.ViewData.Model).Data);
        }

        [Fact]
        public void ShortUrl_should_return_view_result_with_short_url_as_model()
        {
            var alias = new Alias { ShortUrl = new ShortUrl() };
            var shortUrl = new ShortUrlDTO(alias, 3, "http://visiturl.com", "http://previewurl.com");
            
            adminService.Setup(svc => svc.GetShortUrl(It.IsAny<string>())).Returns(shortUrl);

            var view = (ViewResult)controller.ShortUrl(It.IsAny<string>());

            Assert.Same(shortUrl, view.ViewData.Model);
        }

        [Fact]
        public void Users_should_return_view_result_with_users_as_model()
        {
            var users = new Mock<IEnumerable<UserDTO>>();
            
            adminService.Setup(svc => svc.GetUsers()).Returns(users.Object);

            var view = (ViewResult)controller.Users(null, null, null);

            Assert.Same(users.Object, ((GridModel<UserDTO>)view.ViewData.Model).Data);
        }

        [Fact]
        public void Member_should_return_view_result_with_user_as_model()
        {
            var user = new User { ApiSetting = new ApiSetting() };
            var userDto = new UserDTO(user);

            adminService.Setup(svc => svc.GetUser(It.IsAny<long>())).Returns(userDto);

            var view = (ViewResult)controller.Member(It.IsAny<long>());

            Assert.Same(userDto, view.ViewData.Model);
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void BannedIPAddresses_should_return_correct_page(int? page, int expectedCurrentPage)
        {
            var bannedIps = new List<BannedIPAddress>();
            
            for (int i = 0; i < 15; i++)
            {
                bannedIps.Add(new BannedIPAddress());
            }

            adminService.Setup(svc => svc.GetBannedIPAddresses()).Returns(bannedIps);

            var view = (AdaptiveViewResult)controller.BannedIPAddresses(page);
            var model = (PagedListViewModel<BannedIPAddress>)view.ViewData.Model;

            Assert.Equal(expectedCurrentPage, model.CurrentPage);
        }

        [Fact]
        public void CreateBannedIPAddress_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            var result = new Mock<AdministrativeActionResult<BannedIPAddress>>();
            adminService.Setup(svc => svc.CreateBannedIPAddress("127.0.0.1")).Returns(result.Object);

            controller.MockHttpContext("/", "~/ControlPanel/CreateBannedIPAddress/127.0.0.1", "POST");

            string url = controller.Url.BannedIPAddresses(1);
            var view = (AdaptivePostRedirectGetResult)controller.CreateBannedIPAddress("127.0.0.1");
            
            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }

        [Fact]
        public void DeleteBannedIPAddress_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            controller.MockHttpContext("/", "~/ControlPanel/DeleteBannedIPAddress/1", "POST");

            string url = controller.Url.BannedIPAddresses(1);
            var view = (AdaptivePostRedirectGetResult)controller.DeleteBannedIPAddress(It.IsAny<long>());

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }

        [Fact]
        public void DeleteBannedIPAddress_should_use_administrative_service()
        {
            controller.MockHttpContext("/", "~/ControlPanel/DeleteBannedIPAddress/1", "POST");

            controller.DeleteBannedIPAddress(It.IsAny<long>());
            adminService.Verify(svc => svc.DeleteBannedIPAddress(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void CreateBannedDomain_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            var result = new Mock<AdministrativeActionResult<BannedDomain>>();
            adminService.Setup(svc => svc.CreateBannedDomain("pornodomain.com")).Returns(result.Object);

            controller.MockHttpContext("/", "~/ControlPanel/CreateBannedDomain/pornodomain.com", "POST");

            string url = controller.Url.BannedDomains(1);
            var view = (AdaptivePostRedirectGetResult)controller.CreateBannedDomain("pornodomain.com");

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }

        [Fact]
        public void DeleteBannedDomain_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            controller.MockHttpContext("/", "~/ControlPanel/DeleteBannedDomain/1", "POST");

            string url = controller.Url.BannedDomains(1);
            var view = (AdaptivePostRedirectGetResult)controller.DeleteBannedDomain(It.IsAny<long>());

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }

        [Fact]
        public void DeleteBannedDomain_should_use_administrative_service()
        {
            controller.MockHttpContext("/", "~/ControlPanel/DeleteBannedDomain/1", "POST");
            controller.DeleteBannedDomain(It.IsAny<long>());

            adminService.Verify(svc => svc.DeleteBannedDomain(It.IsAny<long>()), Times.Once());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void ReservedAliases_should_return_correct_page(int page, int expectedCurrentPage)
        {
            var aliases = new List<ReservedAlias>();

            for (int i = 0; i < 15; i++)
            {
                aliases.Add(new ReservedAlias());
            }

            adminService.Setup(svc => svc.GetReservedAliases()).Returns(aliases);

            var view = (AdaptiveViewResult)controller.ReservedAliases(page);
            var model = (PagedListViewModel<ReservedAlias>)view.ViewData.Model;

            Assert.Equal(expectedCurrentPage, model.CurrentPage);
        }

        [Fact]
        public void CreateReservedAlias_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            var result = new Mock<AdministrativeActionResult<ReservedAlias>>();
            adminService.Setup(svc => svc.CreateReservedAlias("aliasName")).Returns(result.Object);

            controller.MockHttpContext("/", "~/ControlPanel/CreateReservedAlias/aliasName", "POST");

            string url = controller.Url.ReservedAliases(1);
            var view = (AdaptivePostRedirectGetResult)controller.CreateReservedAlias("aliasName");

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }

        [Fact]
        public void DeleteReservedAlias_should_return_adaptive_post_redirect_get_result_with_correct_url()
        {
            controller.MockHttpContext("/", "~/ControlPanel/DeleteReservedAlias/1", "POST");

            string url = controller.Url.ReservedAliases(1);
            var view = (AdaptivePostRedirectGetResult)controller.DeleteReservedAlias(It.IsAny<long>());

            Assert.Equal(url.ToLower(), view.Url.ToLower());
        }
    }
}