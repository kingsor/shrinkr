namespace Shrinkr.DataTransferObjects
{
    using System;

    using DomainObjects;

    public class VisitDTO
    {
        public VisitDTO(Visit visit)
        {
            Check.Argument.IsNotNull(visit, "visit");

            Id = visit.Id;
            Title = visit.Alias.ShortUrl.Title;
            Url = visit.Alias.ShortUrl.Url;
            Domain = visit.Alias.ShortUrl.Domain;
            SpamStatus = visit.Alias.ShortUrl.SpamStatus;
            Alias = visit.Alias.Name;
            IPAddress = visit.IPAddress;
            Browser = visit.Browser;

            if (visit.Referrer != null)
            {
                ReferrerDomain = visit.Referrer.Domain;
                ReferrerUrl = visit.Referrer.Url;
            }

            CreatedAt = visit.CreatedAt;
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

        public SpamStatus SpamStatus
        {
            get;
            private set;
        }

        public string Alias
        {
            get;
            private set;
        }

        public string IPAddress
        {
            get;
            private set;
        }

        public string Browser
        {
            get;
            private set;
        }

        public string ReferrerDomain
        {
            get;
            private set;
        }

        public string ReferrerUrl
        {
            get;
            private set;
        }

        public DateTime CreatedAt
        {
            get;
            private set;
        }
    }
}