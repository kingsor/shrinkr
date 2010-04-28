namespace Shrinkr.Client.Seemic
{
    using System;
    using System.Windows;
    using System.IO;
    using System.Net;
    using System.ComponentModel.Composition;
    
    using Seesmic.Sdp.Extensibility;

    using Windows;

    [Export(typeof(IShortUrlProvider))]
    public class ShrinkrShortUrlProvider : IShortUrlProvider 
    {
        private static readonly Guid pluginId = new Guid("F32F71BD-807B-4ED3-8E69-212C2BB49B9B");
        private const string rdirApiUrl = "http://rdir.in/api?url={0}&apikey={1}&format=text";
        private static string _apiKey;
        private static string ApiKey
        {
            get
            {
                if (String.IsNullOrEmpty(_apiKey))
                {
                    _apiKey = StorageService.GetValue(pluginId, "apikey", String.Empty);
                }
                return _apiKey;
            }
            set
            {
                _apiKey = value;
                StorageService.SetValue(pluginId, "apikey", value);
            }
        }

        private static IStorageService _storageService;
        public static IStorageService StorageService
        {
            get
            {
                return _storageService;
            }
        }
        
        [Import]
        public IStorageService StorageServiceImport
        {
            set
            {
                _storageService = value;
            }
        }
        
        public DataTemplate Icon
        {
            get { return null; }
        }

        public void ShortUrlAsync(string longUrl, object userState, AsyncCompletedCallback<string> callback)
        {

            if (!String.IsNullOrEmpty(ApiKey))
            {
                ShrinkIt(longUrl, userState, callback);
            }
            else
            {
                var apiKeyInput = new ApiKeyInputWindow();
                apiKeyInput.Show();

                apiKeyInput.ApiKeyReceived += (s, e) =>
                                                  {
                                                      ApiKey = apiKeyInput.ApiKey;
                                                      ShrinkIt(longUrl, userState, callback);
                                                  };
            }
        }

        public string Text
        {
            get { return "rdir.in"; }
        }

        private static Exception ValidateShortUrl(string shortUrl)
        {
            return (shortUrl != null && Uri.IsWellFormedUriString(shortUrl, UriKind.Absolute))
                                  ? null
                                  : new Exception(String.Format("The URL could not be shortened. {0}",shortUrl));

        }

        private void ShrinkIt(string longUrl, object userState, AsyncCompletedCallback<string> callback)
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

                                             callback(this,
                                                      new AsyncCompletedEventArgs<string>(error, false, userState,
                                                                                          shortUrl));
                                         }, null);
        }

    }

}
