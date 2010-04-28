using System;
using System.IO;
using System.Net;

namespace Shrinkr.Client.Desktop.ConsoleApp
{
    class Program
    {
        private enum ResponseFormat
        {
            Text = 1,
            Xml = 2,
            Json = 3
        } ;
        private static string _apiKey;
        private static string _url;
        private static ResponseFormat _format;

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            string cont = "y";
            DialogApiKey();

            while (cont != null && cont.ToLower() == "y")
            {
                DialogUrl();
                DialogFormat();
                ShrinkIt();

                PromptForInput("Do you want shrink another URL? (Y for yes, any other key to end)");
                cont = ReadInput();
            }
        }

        private static void ShrinkIt()
        {
            string rdirApiUrl = String.Format("http://rdir.in/api?url={0}&apikey={1}&format={2}", _url, _apiKey, _format);
            WebRequest request = WebRequest.Create(new Uri(rdirApiUrl));

            WebResponse response = request.GetResponse();

            string content;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                content = sr.ReadToEnd();
            }

            WriteMessage(String.Format("Response: {0} ", content), ConsoleColor.DarkCyan);
        }

        #region Dialog Helpers
        private static void DialogApiKey()
        {
            PromptForInput("Enter your API Key xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx:");
            _apiKey = ReadInput();
            Guid apiKeyGuid;
            while (!Guid.TryParse(_apiKey, out apiKeyGuid))
            {
                PromptError("Please enter correct API Key xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx!");
                _apiKey = ReadInput();
            }
        }

        private static void DialogUrl()
        {
            PromptForInput("Enter long Url to shrink e.g. http://www.microsoft.com/");
            _url = ReadInput();
            while (!Uri.IsWellFormedUriString(_url, UriKind.Absolute))
            {
                PromptError("Please enter well formed URL string!");
                _url = ReadInput();
            }
        }

        private static void DialogFormat()
        {
            PromptForInput("Select response format option (1) Text, (2) XML, (3) JSON. Enter 1, 2 or 3:");
            string selectedOption = ReadInput();
            int parsedOption;
            while (!Int32.TryParse(selectedOption, out parsedOption) || parsedOption < 1 || parsedOption > 3)
            {
                PromptError("Please enter 1 for Text, 2 for XML or 3 for JSON format!");
                selectedOption = ReadInput();
            }
            _format = (ResponseFormat) parsedOption;
        }
        #endregion

        #region Console Helpers
        private static string ReadInput()
        {
            Console.ForegroundColor = ConsoleColor.White;
            return Console.ReadLine();
        }

        private static void WriteMessage(string msg , ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(msg);
        }

        private static void PromptForInput(string msg)
        {
            WriteMessage(msg, ConsoleColor.DarkGreen);
        }

        private static void PromptError(string msg)
        {
            WriteMessage(msg, ConsoleColor.DarkRed);
        }
        #endregion
    }
}
