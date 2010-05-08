namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
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