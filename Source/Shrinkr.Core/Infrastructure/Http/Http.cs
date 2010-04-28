namespace Shrinkr.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;

    using Extensions;

    public class Http : IHttp
    {
        private static string defaultUserAgent = "Shrinkr-Bot";
        private static int defaultTimeout = 15000; // 15 Seconds
        private static int defaultMaximumRedirects = 8;
        private static bool defaultRequestCompressed = true;

        public static string DefaultUserAgent
        {
            get
            {
                return defaultUserAgent;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNullOrEmpty(value, "value");

                defaultUserAgent = value;
            }
        }

        public static int DefaultTimeout
        {
            [DebuggerStepThrough]
            get
            {
                return defaultTimeout;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNegative(value, "value");

                defaultTimeout = value;
            }
        }

        public static int DefaultMaximumRedirects
        {
            [DebuggerStepThrough]
            get
            {
                return defaultMaximumRedirects;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNegative(value, "value");

                defaultMaximumRedirects = value;
            }
        }

        public static bool DefaultRequestCompressed
        {
            [DebuggerStepThrough]
            get
            {
                return defaultRequestCompressed;
            }

            [DebuggerStepThrough]
            set
            {
                defaultRequestCompressed = value;
            }
        }

        public HttpResponse Get(string url, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            WebRequest request = CreateRequest(url, userAgent, timeout, requestCompressed, maximumRedirects, userName, password, contentType, headers, cookies, false);

            return ReadResponse(request);
        }

        public void GetAsync(string url, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            WebRequest request = CreateRequest(url, userAgent, timeout, requestCompressed, maximumRedirects, userName, password, contentType, headers, cookies, false);
            Exception exception = null;

            try
            {
                request.BeginGetResponse(ResponseCallback, new StateContainer { Request = request, OnComplete = onComplete, OnError = onError });
            }
            catch (Exception e)
            {
                exception = e;
            }

            if ((exception != null) && (onError != null))
            {
                onError(exception);
            }
        }

        public HttpResponse Post(string url, string data, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            WebRequest request = CreateRequest(url, userAgent, timeout, requestCompressed, maximumRedirects, userName, password, contentType, headers, cookies, true);

            if (!string.IsNullOrEmpty(data))
            {
                byte[] content = Encoding.ASCII.GetBytes(data);
                request.ContentLength = content.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(content, 0, content.Length);
                }
            }

            return ReadResponse(request);
        }

        public void PostAsync(string url, IDictionary<string, string> formFields, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            string rawData = ConvertFormFieldsToString(formFields);

            PostAsync(url, rawData, userAgent, timeout, requestCompressed, maximumRedirects, userName, password, contentType, headers, cookies, onComplete, onError);
        }

        public void PostAsync(string url, string data, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, Action<HttpResponse> onComplete, Action<Exception> onError)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            WebRequest request = CreateRequest(url, userAgent, timeout, requestCompressed, maximumRedirects, userName, password, contentType, headers, cookies, true);

            byte[] content = !string.IsNullOrEmpty(data) ? Encoding.ASCII.GetBytes(data) : new byte[0];

            request.ContentLength = content.Length;

            Exception exception = null;

            try
            {
                request.BeginGetRequestStream(RequestCallback, new StateContainer { Request = request, RequestContent = content, OnComplete = onComplete, OnError = onError });
            }
            catch (Exception e)
            {
                exception = e;
            }

            if ((exception != null) && (onError != null))
            {
                onError(exception);
            }
        }

        protected virtual WebRequest CreateRequest(string url, string userAgent, int timeout, bool requestCompressed, int maximumRedirects, string userName, string password, string contentType, IDictionary<string, string> headers, IDictionary<string, string> cookies, bool isPost)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(url));

            request.Method = isPost ? "POST" : "GET";
            request.UserAgent = userAgent;

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                request.Credentials = new NetworkCredential(userName, password);
            }

            if (maximumRedirects > 0)
            {
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = maximumRedirects;
            }
            else
            {
                request.AllowAutoRedirect = false;
            }

            request.Accept = "*/*";
            request.Expect = string.Empty;

            if (!headers.IsNullOrEmpty())
            {
                foreach (KeyValuePair<string, string> pair in headers)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }
            }

            if (!cookies.IsNullOrEmpty())
            {
                request.CookieContainer = new CookieContainer();

                foreach (KeyValuePair<string, string> pair in cookies)
                {
                    request.CookieContainer.Add(new Cookie(pair.Key, pair.Value));
                }
            }

            if (timeout > 0)
            {
                request.Timeout = timeout;
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                contentType = "application/x-www-form-urlencoded; charset=utf-8";
            }

            request.ContentType = contentType;

            if (requestCompressed)
            {
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return request;
        }

        private static void RequestCallback(IAsyncResult result)
        {
            StateContainer states = (StateContainer) result.AsyncState;
            Exception exception = null;

            try
            {
                states.RequesteStream = states.Request.EndGetRequestStream(result);
                states.RequesteStream.BeginWrite(states.RequestContent, 0, states.RequestContent.Length, WriteCallback, states);
            }
            catch (Exception e)
            {
                exception = e;
            }

            OnException(states, exception);
        }

        private static void WriteCallback(IAsyncResult result)
        {
            StateContainer states = (StateContainer) result.AsyncState;
            Exception exception = null;

            try
            {
                states.RequesteStream.EndWrite(result);
                states.Request.BeginGetResponse(ResponseCallback, states);
            }
            catch (Exception e)
            {
                exception = e;
            }

            OnException(states, exception);
        }

        private static void ResponseCallback(IAsyncResult result)
        {
            const int BufferLength = 8096;

            IDictionary<string, string> headers = new Dictionary<string, string>();
            IDictionary<string, string> cookies = new Dictionary<string, string>();

            StateContainer states = (StateContainer) result.AsyncState;
            Exception exception = null;

            try
            {
                HttpWebResponse response = (HttpWebResponse) states.Request.EndGetResponse(result);

                if (response.StatusCode < HttpStatusCode.OK && response.StatusCode >= HttpStatusCode.Ambiguous)
                {
                    throw new WebException(TextMessages.UnableToHandleTheRequest, null, WebExceptionStatus.UnknownError, response);
                }

                string contentType = response.ContentType;

                PopulateHeaders(response, headers);
                PopulateCookies(response, cookies);

                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[BufferLength];

                    string content;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        int bytesRead;

                        while ((bytesRead = stream.Read(buffer, 0, BufferLength)) > 0)
                        {
                            ms.Write(buffer, 0, bytesRead);
                        }

                        ms.Flush();

                        Encoding encoding = DetectEncoding(contentType, Encoding.UTF8);

                        content = encoding.GetString(ms.ToArray());
                    }

                    HttpResponse httpResponse = CreateHttpResponseWith(contentType, content, headers, cookies);

                    if (states.OnComplete != null)
                    {
                        states.OnComplete(httpResponse);
                    }
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            OnException(states, exception);
        }

        private static string ConvertFormFieldsToString(ICollection<KeyValuePair<string, string>> formFields)
        {
            StringBuilder requestBody = new StringBuilder();

            if (!formFields.IsNullOrEmpty())
            {
                foreach (KeyValuePair<string, string> pair in formFields)
                {
                    if (requestBody.Length > 0)
                    {
                        requestBody.Append("&");
                    }

                    requestBody.Append("{0}={1}".FormatWith(pair.Key, pair.Value));
                }
            }

            return requestBody.ToString();
        }

        private static HttpResponse ReadResponse(WebRequest request)
        {
            string contentType;
            string content;

            IDictionary<string, string> headers = new Dictionary<string, string>();
            IDictionary<string, string> cookies = new Dictionary<string, string>();

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                if (response.StatusCode < HttpStatusCode.OK && response.StatusCode >= HttpStatusCode.Ambiguous)
                {
                    throw new WebException(TextMessages.UnableToHandleTheRequest, null, WebExceptionStatus.UnknownError, response);
                }

                contentType = response.ContentType;

                PopulateHeaders(response, headers);
                PopulateCookies(response, cookies);

                Encoding encoding = DetectEncoding(contentType, Encoding.UTF8);

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    content = sr.ReadToEnd();
                }
            }

            return CreateHttpResponseWith(contentType, content, headers, cookies);
        }

        private static void PopulateHeaders(HttpWebResponse httpResponse, IDictionary<string, string> headers)
        {
            foreach (string key in httpResponse.Headers)
            {
                headers.Add(key, httpResponse.GetResponseHeader(key));
            }
        }

        private static void PopulateCookies(HttpWebResponse httpResponse, IDictionary<string, string> cookies)
        {
            foreach (Cookie cookie in httpResponse.Cookies)
            {
                cookies.Add(cookie.Name, cookie.Value);
            }
        }

        private static Encoding DetectEncoding(string contentType, Encoding defaultEncoding)
        {
            Encoding encoding = defaultEncoding;

            if (!string.IsNullOrEmpty(contentType))
            {
                bool found = false;

                foreach (string part in contentType.ToLower(Culture.Invariant).Split(new[] { ';', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (found)
                    {
                        try
                        {
                            encoding = Encoding.GetEncoding(part);
                        }
                        catch (ArgumentException)
                        {
                        }

                        break;
                    }

                    if (string.Compare(part, "charset", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        found = true;
                    }
                }
            }

            return encoding;
        }

        private static void OnException(StateContainer states, Exception exception)
        {
            if (exception != null)
            {
                if (states.RequesteStream != null)
                {
                    states.RequesteStream.Close();
                }

                if (states.OnError != null)
                {
                    states.OnError(exception);
                }
            }
        }

        private static HttpResponse CreateHttpResponseWith(string contentType, string content, IEnumerable<KeyValuePair<string, string>> headers, IEnumerable<KeyValuePair<string, string>> cookies)
        {
            HttpResponse httpResponse = new HttpResponse(content, contentType);

            httpResponse.Headers.AddRange(headers);
            httpResponse.Cookies.AddRange(cookies);

            return httpResponse;
        }

        private sealed class StateContainer
        {
            public WebRequest Request
            {
                get;
                set;
            }

            public Stream RequesteStream
            {
                get;
                set;
            }

            public byte[] RequestContent
            {
                get;
                set;
            }

            public Action<HttpResponse> OnComplete
            {
                get;
                set;
            }

            public Action<Exception> OnError
            {
                get;
                set;
            }
        }
    }
}