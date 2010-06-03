namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using FluentNHibernate.Mapping;

    using DomainObjects;

    public class BannedDomainMap : ClassMap<BannedDomain>
    {
        public BannedDomainMap()
        {
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Name).Not.Nullable().Length(440);

            Table("BannedDomain");
        }
    }
}