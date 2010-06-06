namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using FluentNHibernate.Mapping;
  
    using DomainObjects;

    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap()
        {
            Table("Visit");

            Id(v => v.Id).GeneratedBy.Identity();

            Map(v => v.IPAddress).Not.Nullable().Length(15);
            Map(v => v.Browser).Nullable().Length(440);
            Map(v => v.GeoCode).Nullable();
            Map(v => v.CreatedAt).Not.Nullable();

            Component(
                        a =>
                            a.Referrer,
                            m =>
                            {
                                m.Map(r => r.Domain).Nullable().Length(440).Column("ReferrerDomain");
                                m.Map(r => r.Url).Nullable().Length(2048).Column("ReferrerUrl");
                            }).Not.LazyLoad();

            References(v => v.Alias).Not.Nullable().Column("AliasId").LazyLoad();
        }
    }
}