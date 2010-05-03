namespace Shrinkr.Infrastructure.Nhibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class BadWordMap : ClassMap<BadWord>
    {
        public BadWordMap()
        {
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Expression).Not.Nullable().Length(440);

            Table("BadWord");
        }
    }
}