namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using System;

    using FluentNHibernate.Mapping;

    using DomainObjects;

    [CLSCompliant(false)]
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(u => u.Id).GeneratedBy.Identity();

            Map(u => u.Name).Not.Nullable().Length(256);
            Map(u => u.Email).Nullable().Length(256);
            Map(u => u.IsLockedOut).Not.Nullable();
            Map(u => u.CreatedAt).Not.Nullable();
            Map(u => u.InternalRole).Not.Nullable().Column("Role");
            Map(u => u.LastActivityAt).Not.Nullable();

            Table("User");
        }
    }
}