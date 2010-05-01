namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class BannedIPAddressMap : ClassMap<BannedIPAddress>
    {
        public BannedIPAddressMap()
        {
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.IPAddress).Not.Nullable().Length(15);

            Table("BannedIPAddress");
        }
    }
}