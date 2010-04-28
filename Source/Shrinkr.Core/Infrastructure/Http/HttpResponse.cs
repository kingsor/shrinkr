namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class HttpResponse
    {
        public HttpResponse(string content, string contentType)
        {
            Content = content;
            ContentType = contentType;

            Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Cookies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Content
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public IDictionary<string, string> Headers
        {
            get;
            private set;
        }

        public IDictionary<string, string> Cookies
        {
            get;
            private set;
        }
    }
}