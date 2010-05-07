namespace Shrinkr.Client.Console
{
    using System;
    using System.IO;
    using System.Net;

    public static class Program
    {
        private enum ResponseFormat
        {
            Text = 1,
            Xml = 2,
            Json = 3
        }

        private static string apiKey;
        private static string url;
        private static ResponseFormat format;

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
            string rdirApiUrl = string.Format("http://rdir.in/api?url={0}&apikey={1}&format={2}", url, apiKey, format);

            WebRequest request = WebRequest.Create(new Uri(rdirApiUrl));

            WebResponse response = request.GetResponse();

            string content;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                content = sr.ReadToEnd();
            }

            WriteMessage(string.Format("Response: {0} ", content), ConsoleColor.DarkCyan);
        }

        private static void DialogApiKey()
        {
            PromptForInput("Enter your API Key xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx:");

            apiKey = ReadInput();

            Guid apiKeyGuid;

            while (!Guid.TryParse(apiKey, out apiKeyGuid))
            {
                PromptError("Please enter correct API Key xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx!");
                apiKey = ReadInput();
            }
        }

        private static void DialogUrl()
        {
            PromptForInput("Enter long Url to shrink e.g. http://www.microsoft.com/");

            url = ReadInput();

            while (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                PromptError("Please enter well formed URL string!");
                url = ReadInput();
            }
        }

        private static void DialogFormat()
        {
            PromptForInput("Select response format option (1) Text, (2) XML, (3) JSON. Enter 1, 2 or 3:");

            string selectedOption = ReadInput();

            int parsedOption;

            while (!int.TryParse(selectedOption, out parsedOption) || parsedOption < (int) ResponseFormat.Text || parsedOption > (int) ResponseFormat.Json)
            {
                PromptError("Please enter 1 for Text, 2 for XML or 3 for JSON format!");

                selectedOption = ReadInput();
            }

            format = (ResponseFormat) parsedOption;
        }

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
    }
}