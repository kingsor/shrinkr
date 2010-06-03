namespace Shrinkr.DataTransferObjects
{
    using System;

    using DomainObjects;

    public class ShortUrlDTO
    {
        public ShortUrlDTO(Alias alias, int visits, string visitUrl, string previewUrl)
        {
            Check.Argument.IsNotNull(alias, "alias");
            Check.Argument.IsNotNullOrEmpty(visitUrl, "visitUrl");
            Check.Argument.IsNotNullOrEmpty(previewUrl, "previewUrl");

            Id = alias.ShortUrl.Id;
            Title = alias.ShortUrl.Title;
            Url = alias.ShortUrl.Url;
            Domain = alias.ShortUrl.Domain;
            Alias = alias.Name;
            CreatedAt = alias.CreatedAt;
            SpamStatus = alias.ShortUrl.SpamStatus;
            Visits = visits;

            if (alias.User != null)
            {
                UserId = alias.User.Id;
                UserName = alias.User.Name;
            }

            IPAddress = alias.IPAddress;
            VisitUrl = visitUrl;
            PreviewUrl = previewUrl;
        }

        public long Id
        {
            get;
            private set;
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

        public string Domain
        {
            get;
            private set;
        }

        public string Alias
        {
            get;
            private set;
        }

        public DateTime CreatedAt
        {
            get;
            private set;
        }

        public SpamStatus SpamStatus
        {
            get;
            private set;
        }

        public int Visits
        {
            get;
            private set;
        }

        public long UserId
        {
            get;
            private set;
        }

        public string UserName
        {
            get;
            private set;
        }

        public string IPAddress
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
    }
}