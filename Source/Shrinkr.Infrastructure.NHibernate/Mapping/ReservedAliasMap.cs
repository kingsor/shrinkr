namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class ReservedAliasMap : ClassMap<ReservedAlias>
    {
        public ReservedAliasMap()
        {
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Name).Not.Nullable().Length(440);

            Table("ReservedAlias");
        }
    }
}