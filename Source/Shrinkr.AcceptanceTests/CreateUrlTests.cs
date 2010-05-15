namespace Shrinkr.AcceptanceTests
{
    using System;

    using WatiN.Core;
    using Xunit;

    public class When_short_url_is_created_without_alias : IDisposable
    {
        private readonly Browser browser = TestHelper.CreateBrowser();

        public When_short_url_is_created_without_alias()
        {
            browser.GoTo(TestHelper.ApplicationUrl);
            browser.TextField(Find.ByName("url")).Value = "http://www.dotnetrocks.com/default.aspx?showNum=1";
            browser.Button(Find.ByValue("Shrink")).Click();

            browser.WaitForComplete();
        }

        public void Dispose()
        {
            browser.Close();
            browser.Dispose();
        }

        [Fact]
        public void Should_contain_the_success_message()
        {
            var message = browser.Div(Find.ByClass("messageBox")).Elements[0].Text;

            Assert.Equal(message, "Your new urls are generated successfully.");
        }

        [Fact]
        public void Should_contain_the_shrinked_url()
        {
            var shrinkedUrl = browser.TextField(Find.ById("newUrl")).Value;

            Assert.True(shrinkedUrl.StartsWith(TestHelper.ApplicationUrl + "/"));
        }

        [Fact]
        public void Should_contain_the_preview_url()
        {
            var shrinkedUrl = browser.TextField(Find.ById("previewUrl")).Value;

            Assert.True(shrinkedUrl.StartsWith(TestHelper.ApplicationUrl + "/p/"));
        }
    }
}