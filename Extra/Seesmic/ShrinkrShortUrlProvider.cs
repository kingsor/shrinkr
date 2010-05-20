namespace Shrinkr.Client.Seemic
{
    using System;
    using System.Windows;
    using System.IO;
    using System.Net;
    using System.ComponentModel.Composition;
    
    using Seesmic.Sdp.Extensibility;

    [Export(typeof(IShortUrlProvider))]
    public class ShrinkrShortUrlProvider : IShortUrlProvider
    {
        private const string rdirApiUrl = "http://rdir.in/api?url={0}&apikey={1}&format=text";

        private static readonly Guid pluginId = new Guid("395055B6-2FE7-4617-A0B2-38C856556C3B");
        private static IStorageService storageService;

        private static string apiKey;

        private static string ApiKey
        {
            get
            {
                if (String.IsNullOrEmpty(apiKey))
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
                return null;
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
                var apiKeyInput = new ApiKeyInputWindow();

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
                                  : new Exception(String.Format("The URL could not be shortened. {0}",shortUrl));
        }

        private void Shrink(string longUrl, object userState, AsyncCompletedCallback<string> callback)
        {
            string requestUrl = String.Format(rdirApiUrl, longUrl, ApiKey);
            WebRequest request = WebRequest.Create(new Uri(requestUrl));

            request.BeginGetResponse(s =>
                                         {
                                             Exception error;
                                             string shortUrl = String.Empty;

                                             try
                                             {
                                                 WebResponse response = request.EndGetResponse(s);

                                                 using (var sr = new StreamReader(response.GetResponseStream()))
                                                 {
                                                     shortUrl = sr.ReadToEnd();
                                                 }

                                                 error = ValidateShortUrl(shortUrl);

                                                 if (error != null && error.Message.Contains("ApiKey"))
                                                 {
                                                     ApiKey = String.Empty;
                                                 }
                                             }
                                             catch (Exception ex)
                                             {
                                                 error = ex;
                                             }

                                             callback(this, new AsyncCompletedEventArgs<string>(error, false, userState, shortUrl));

                                         }, null);
        }
    }
}