namespace Shrinkr.Infrastructure.NHibernate.Mapping
{
    using FluentNHibernate.Mapping;
  
    using DomainObjects;
   
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("[User]");

            Id(u => u.Id).GeneratedBy.Identity();

            Map(u => u.Name).Not.Nullable().Length(256);
            Map(u => u.Email).Nullable().Length(256);
            Map(u => u.IsLockedOut).Not.Nullable();
            Map(u => u.CreatedAt).Not.Nullable();
            Map(u => u.InternalRole).Not.Nullable().Column("Role");
            Map(u => u.LastActivityAt).Not.Nullable();

            Component(
                        u =>
                            u.ApiSetting,
                            m =>
                                {
                                    m.Map(s => s.Key).Nullable().Length(36).Column("ApiKey");
                                    m.Map(s => s.Allowed).Nullable().Column("ApiAllowed");
                                    m.Map(s => s.DailyLimit).Nullable();
                                }).Not.LazyLoad();

            HasMany(u => u.Aliases).KeyColumn("UserId").Access.LowerCaseField().LazyLoad().Cascade.All();
        }
    }
}