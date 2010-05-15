namespace Shrinkr.AcceptanceTests
{
    using System;

    using WatiN.Core;
    using Xunit;

    public abstract class UrlCreatedAsAnonymousUser : IDisposable
    {
        protected UrlCreatedAsAnonymousUser()
        {
            Browser = TestHelper.CreateBrowser();

            Browser.GoTo(TestHelper.ApplicationUrl);
            SetFormValues();

            var submitButton = Browser.Button(Find.ByValue("Shrink"));

            submitButton.Click();
            submitButton.WaitUntil(btn => btn.Enabled);
        }

        protected Browser Browser
        {
            get;
            private set;
        }

        public void Dispose()
        {
            Browser.Close();
            Browser.Dispose();
        }

        [Fact]
        public void Should_contain_the_success_message()
        {
            var message = Browser.Div(Find.ByClass("messageBox")).Elements[0].Text;

            Assert.Equal(message, "Your new urls are generated successfully.");
        }

        protected abstract void SetFormValues();

        protected void SetFormValues(string url, string alias)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Browser.TextField(Find.ByName("url")).Value = url;
            }

            if (!string.IsNullOrEmpty(alias))
            {
                Browser.TextField(Find.ByName("alias")).Value = alias;
            }
        }
    }

    public class When_short_url_is_created_without_alias : UrlCreatedAsAnonymousUser
    {
        protected override void SetFormValues()
        {
            SetFormValues(string.Format("http://www.dotnetrocks.com/default.aspx?showNum={0}", new Random().Next(1, 558)), null);
        }

        [Fact]
        public void Should_contain_the_shrinked_url()
        {
            var shrinkedUrl = Browser.TextField(Find.ById("newUrl")).Value;

            Assert.True(shrinkedUrl.StartsWith(TestHelper.ApplicationUrl + "/"));
        }

        [Fact]
        public void Should_contain_the_preview_url()
        {
            var shrinkedUrl = Browser.TextField(Find.ById("previewUrl")).Value;

            Assert.True(shrinkedUrl.StartsWith(TestHelper.ApplicationUrl + "/p/"));
        }
    }

    public class When_short_url_is_created_with_alias : UrlCreatedAsAnonymousUser
    {
        private readonly string alias = CreateRandomAlias();

        [Fact]
        public void Should_contain_the_shrinked_url()
        {
            var shrinkedUrl = Browser.TextField(Find.ById("newUrl")).Value;

            Assert.Equal(TestHelper.ApplicationUrl + "/" + alias, shrinkedUrl);
        }

        [Fact]
        public void Should_contain_the_preview_url()
        {
            var shrinkedUrl = Browser.TextField(Find.ById("previewUrl")).Value;

            Assert.Equal(TestHelper.ApplicationUrl + "/p/" + alias, shrinkedUrl);
        }

        protected override void SetFormValues()
        {
            SetFormValues(string.Format("http://www.dotnetrocks.com/default.aspx?showNum={0}", new Random().Next(1, 558)), alias);
        }

        private static string CreateRandomAlias()
        {
            var rnd = new Random();
            var builder = string.Empty;

            for (int i = 1; i <= rnd.Next(2, 24); i++)
            {
                builder += ((char)rnd.Next(65, 90)).ToString();
            }

            return builder;
        }
    }
}