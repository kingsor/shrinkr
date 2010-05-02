namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class ShortUrlMap : ClassMap<ShortUrl>
    {
        public ShortUrlMap()
        {
            Id(s => s.Id).GeneratedBy.Identity();

            Map(s => s.Url).Not.Nullable().Length(2048);
            Map(s => s.Domain).Not.Nullable().Length(440);
            Map(s => s.Hash).Not.Nullable().Length(24);
            Map(s => s.Title).Not.Nullable().Length(2048);
            Map(s => s.InternalSpamStatus).Not.Nullable().Column("SpamStatus");

            HasMany(s => s.Aliases).KeyColumn("ShortUrlId");

            Table("ShortUrl");
        }
    }
}
