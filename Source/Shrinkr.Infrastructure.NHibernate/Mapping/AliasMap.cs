﻿namespace Shrinkr.Infrastructure.Nhibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class AliasMap : ClassMap<Alias>
    {
        public AliasMap()
        {
            Id(a => a.Id).GeneratedBy.Identity();
            
            Map(a => a.Name).Not.Nullable().Length(440);
            Map(a => a.IPAddress).Not.Nullable().Length(15);
            Map(a => a.CreatedAt).Not.Nullable();
            Map(a => a.CreatedByApi).Not.Nullable();
            
            References(a => a.User).Nullable().Column("UserId");
            References(a => a.ShortUrl).Not.Nullable().Column("ShortUrlId");
            HasMany(a => a.Visits).KeyColumn("AliasId");
            
            Table("Alias");
        }
    }
}
