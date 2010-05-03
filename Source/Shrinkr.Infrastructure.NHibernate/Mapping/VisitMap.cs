namespace Shrinkr.Infrastructure.Nhibernate.Mapping
{
    using System;
  
    using FluentNHibernate.Mapping;
  
    using DomainObjects;

    [CLSCompliant(false)]
    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap()
        {
            Id(v => v.Id).GeneratedBy.Identity();

            Map(v => v.IPAddress).Not.Nullable().Length(15);
            Map(v => v.Browser).Nullable().Length(440);
            Map(v => v.GeoCode).Nullable();
            Map(v => v.CreatedAt).Not.Nullable();

            Component(a => a.Referrer, m =>
                                           {
                                               m.Map(r => r.Domain).Nullable().Length(440).Column("ReferrerDomain");
                                               m.Map(r => r.Url).Nullable().Length(2048).Column("ReferrerUrl");
                                           });

            References(v => v.Alias).Not.Nullable().Column("AliasId");

            Table("Visit");
        }
    }
}
