namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using FluentNHibernate.Mapping;

    using DomainObjects;

    public class BadWordMap : ClassMap<BadWord>
    {
        public BadWordMap()
        {
            Table("BadWord");

            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Expression).Not.Nullable().Length(440);
        }
    }
}