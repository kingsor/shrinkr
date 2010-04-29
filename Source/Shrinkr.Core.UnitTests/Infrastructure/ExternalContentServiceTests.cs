namespace Shrinkr.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Infrastructure;

    using Moq;
    using Xunit;

    public class ExternalContentServiceTests
    {
        private readonly Mock<IHttp> http;
        private readonly Mock<ICacheManager> cacheManager;

        private readonly ExternalContentService externalContentService;

        public ExternalContentServiceTests()
        {
            http = new Mock<IHttp>();
            cacheManager = new Mock<ICacheManager>();

            externalContentService = new ExternalContentService(http.Object, cacheManager.Object);
        }

        [Fact]
        public void Should_be_able_to_retrieve_external_content()
        {
            const string Html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" " +
                                @"""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">" + "\r\n" +
                                @"<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">" + "\r\n" +
                                @"<head><meta http-equiv=""content-type"" content=""text/html;charset=utf-8"" /><title>" +
                                "\r\n" +
                                "\t" + "\r\n" +
                                @"    Shrinkr" + "\r\n" +
                                @" " + "\r\n" +
                                @"</title><meta name=""Rating"" content=""General"" /><meta name=""robots"" content=""index, follow"" />" +
                                "\r\n" +
                                @"    <link href=""/Shrinkr/asset.axd?name=css&amp;v=0.0.0.1"" rel=""stylesheet"" type=""text/css""/>" +
                                "\r\n" +
                                @"    <script src=""/Shrinkr/asset.axd?name=jQuery&amp;v=0.0.0.1"" type=""text/javascript""></script>" +
                                "\r\n" +
                                @"    <script src=""/Shrinkr/asset.axd?name=scripts&amp;v=0.0.0.1"" type=""text/javascript""></script>" +
                                "\r\n" +
                                @"</head>" + "\r\n" +
                                @"<body>" + "\r\n" +
                                @"    <div class=""page"">" + "\r\n" +
                                @"        <div id=""header"">" + "\r\n" +
                                @"            <div id=""title"">" + "\r\n" +
                                @"                <h1>Shrinkr</h1>" + "\r\n" +
                                @"            </div>" + "\r\n" +
                                @"            <div id=""logindisplay"">" + "\r\n" +
                                @"                " + "\r\n" +
                                @"    [<a href=""/Shrinkr/LogOn"">Log On</a> ]" + "\r\n" +
                                @" " + "\r\n" +
                                @"            </div> " + "\r\n" +
                                @"            <div id=""menucontainer"">" + "\r\n" +
                                @"                <ul id=""menu"">" + "\r\n" +
                                @"                    <li class=""selected""><a href=""/Shrinkr/"">Home</a></li>" + "\r\n" +
                                @"                    <li><a href=""/Shrinkr/Home/About"">About Us</a></li>" + "\r\n" +
                                @"                </ul>" + "\r\n" +
                                @"            </div>" + "\r\n" +
                                @"        </div>" + "\r\n" +
                                @"        <div id=""main"">" + "\r\n" +
                                @"            " + "\r\n" +
                                @"    <h2>Welcome to ASP.NET MVC!</h2>" + "\r\n" +
                                @"    <p>" + "\r\n" +
                                @"        To learn more about ASP.NET MVC visit <a href=""http://asp.net/mvc"" title=""ASP.NET MVC " +
                                @"Website"">http://asp.net/mvc</a>." + "\r\n" +
                                @"    </p>" + "\r\n" +
                                @" " + "\r\n" +
                                @"            <div id=""footer"">" + "\r\n" +
                                @"                Shrinkr &copy; Copyright 2009" + "\r\n" +
                                @"            </div>" + "\r\n" +
                                @"        </div>" + "\r\n" +
                                @"    </div>" + "\r\n" +
                                @"</body>" + "\r\n" +
                                @"</html>" + "\r\n";

            cacheManager.Setup(cm => cm.GetOrCreate(It.IsAny<string>(), It.IsAny<Func<ExternalContent>>())).Returns((string key, Func<ExternalContent> callback) => callback());

            http.Setup(h => h.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<IDictionary<string, string>>())).Returns(new HttpResponse(Html, "text/html"));

            ExternalContent externalContent = externalContentService.Retrieve("http://shrinkr.com");

            Assert.Equal("Shrinkr", externalContent.Title);
        }

        [Fact]
        public void Should_throw_exception_when_empty_content_is_returned_with_unexpected_status_code()
        {
            cacheManager.Setup(cm => cm.GetOrCreate(It.IsAny<string>(), It.IsAny<Func<ExternalContent>>())).Returns((string key, Func<ExternalContent> callback) => callback());
            http.Setup(h => h.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<IDictionary<string, string>>())).Throws(new WebException("Unable to connect."));

            Assert.Throws<WebException>(() => externalContentService.Retrieve("http://shrinkr.com"));
        }
    }
}