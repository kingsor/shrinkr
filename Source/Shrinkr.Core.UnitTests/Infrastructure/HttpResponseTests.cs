namespace Shrinkr.UnitTests
{
    using Infrastructure;

    using Xunit;

    public class HttpResponseTests
    {
        private readonly HttpResponse httpResponse;

        public HttpResponseTests()
        {
            httpResponse = new HttpResponse("This is a dummy content", "text/html");
        }

        [Fact]
        public void Content_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("This is a dummy content", httpResponse.Content);
        }

        [Fact]
        public void ContentType_should_be_same_which_is_passed_in_constructor()
        {
            Assert.Equal("text/html", httpResponse.ContentType);
        }

        [Fact]
        public void Headers_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(httpResponse.Headers);
        }

        [Fact]
        public void Cookies_should_be_empty_when_new_instance_is_created()
        {
            Assert.Empty(httpResponse.Cookies);
        }
    }
}