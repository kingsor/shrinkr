namespace Shrinkr.Web
{
    using System;
    using System.Diagnostics;

    using DataTransferObjects;

    public class CreateUrlViewModel
    {
        public CreateUrlViewModel(ShortUrlDTO shortUrl)
        {
            Check.Argument.IsNotNull(shortUrl, "shortUrl");

            Title = shortUrl.Title;
            Url = shortUrl.Url;
            Alias = shortUrl.Alias;
            VisitUrl = shortUrl.VisitUrl;
            PreviewUrl = shortUrl.PreviewUrl;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            private set;
        }

        public string Alias
        {
            get;
            private set;
        }

        public string VisitUrl
        {
            get;
            private set;
        }

        public string PreviewUrl
        {
            get;
            private set;
        }

        public bool HasReduced
        {
            [DebuggerStepThrough]
            get
            {
                return VisitUrl.Length < Url.Length;
            }
        }

        public float ReducedPercent
        {
            [DebuggerStepThrough]
            get
            {
                int difference = Url.Length - VisitUrl.Length;

                double percent = (difference * 100) / Url.Length;

                float result = Convert.ToSingle(Math.Abs(percent));

                return result;
            }
        }

        public int ReducedCharacters
        {
            [DebuggerStepThrough]
            get
            {
                int difference = Url.Length - VisitUrl.Length;
                int result = Math.Abs(difference);

                return result;
            }
        }
    }
}