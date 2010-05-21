namespace Shrinkr.Client.Seemic
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Net;
    using System.Windows;

    using Seesmic.Sdp.Extensibility;

    [Export(typeof(IShortUrlProvider))]
    public class ShrinkrShortUrlProvider : IShortUrlProvider
    {
        private const string Endpoint = "http://rdir.in/api?url={0}&apikey={1}&format=text";

        private static readonly Guid pluginId = new Guid("395055B6-2FE7-4617-A0B2-38C856556C3B");

        private static string apiKey;
        private static IStorageService storageService;

        public static IStorageService StorageService
        {
            get
            {
                return storageService;
            }
        }

        [Import]
        public IStorageService StorageServiceImport
        {
            set
            {
                storageService = value;
            }
        }

        public string ApiKey
        {
            get
            {
                if (string.IsNullOrEmpty(apiKey))
                {
                    apiKey = StorageService.GetValue(pluginId, "apikey", string.Empty);
                }

                return apiKey;
            }

            set
            {
                apiKey = value;
                StorageService.SetValue(pluginId, "apikey", value);
            }
        }

        public string Id
        {
            get
            {
                return pluginId.ToString();
            }
        }

        public DataTemplate Icon
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Text
        {
            get { return "rdir.in"; }
        }

        public void ShortUrlAsync(string longUrl, object userState, AsyncCompletedCallback<string> callback)
        {
            if (!string.IsNullOrEmpty(ApiKey))
            {
                Shrink(longUrl, userState, callback);
            }
            else
            {
                ApiKeyInputWindow apiKeyInput = new ApiKeyInputWindow();

                apiKeyInput.Show();

                apiKeyInput.ApiKeyReceived += (s, e) =>
                                                  {
                                                      ApiKey = apiKeyInput.ApiKey;
                                                      Shrink(longUrl, userState, callback);
                                                  };
            }
        }

        private static Exception ValidateShortUrl(string shortUrl)
        {
            return (shortUrl != null && Uri.IsWellFormedUriString(shortUrl, UriKind.Absolute))
                                  ? null
                                  : new Exception(String.Format("The URL could not be shortened. {0}", shortUrl));
        }

        private void Shrink(string longUrl, object userState, AsyncCompletedCallback<string> callback)
        {
            string requestUrl = String.Format(Endpoint, longUrl, ApiKey);
            WebRequest request = WebRequest.Create(new Uri(requestUrl));

            request.BeginGetResponse(
                                      s =>
                                         {
                                             Exception error;
                                             string shortUrl = String.Empty;

                                             try
                                             {
                                                 WebResponse response = request.EndGetResponse(s);

                                                 using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                                 {
                                                     shortUrl = sr.ReadToEnd();
                                                 }

                                                 error = ValidateShortUrl(shortUrl);

                                                 if (error != null && error.Message.Contains("ApiKey"))
                                                 {
                                                     ApiKey = string.Empty;
                                                 }
                                             }
                                             catch (Exception ex)
                                             {
                                                 error = ex;
                                             }

                                             callback(this, new AsyncCompletedEventArgs<string>(error, false, userState, shortUrl));
                                         },
                                         null);
        }
    }
}