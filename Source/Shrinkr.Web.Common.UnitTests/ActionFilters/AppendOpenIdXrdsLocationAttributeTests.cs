namespace Shrinkr.Web.UnitTests
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class AppendOpenIdXrdsLocationAttributeTests
    {
        private readonly AppendOpenIdXrdsLocationAttribute attribute;

        public AppendOpenIdXrdsLocationAttributeTests()
        {
            attribute = new AppendOpenIdXrdsLocationAttribute();
        }

        [Fact]
        public void OnResultExecuted_should_add_xrds_location_header_to_response()
        {
            var httpContext = MvcTestHelper.CreateHttpContext("/", "~/LogOn", null);

            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            var actionExcutingContext = new ResultExecutedContext(controllerContext, new Mock<ActionResult>().Object, false, null);

            new RegisterRoutes().Execute();

            attribute.OnResultExecuted(actionExcutingContext);

            httpContext.Verify(c => c.Response.AddHeader(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}