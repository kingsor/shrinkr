namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using FluentNHibernate.Mapping;

    using DomainObjects;
   
    public class ReservedAliasMap : ClassMap<ReservedAlias>
    {
        public ReservedAliasMap()
        {
            Table("ReservedAlias");

            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Name).Not.Nullable().Length(440);
        }
    }
}