namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class UserConfiguration : EntityConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.Id);

            Property(u => u.Id).IsIdentity();
            Property(u => u.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(256);
            Property(u => u.Email).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(256);
            Property(u => u.IsLockedOut);
            Property(u => u.CreatedAt);
            Property(u => u.InternalRole);
            Property(u => u.LastActivityAt);

            MapSingleType(u => new
                                   {
                                       u.Id,
                                       u.Name,
                                       u.Email,
                                       u.IsLockedOut,
                                       u.CreatedAt,
                                       Role = u.InternalRole,
                                       ApiKey = u.ApiSetting.Key,
                                       ApiAllowed = u.ApiSetting.Allowed,
                                       u.ApiSetting.DailyLimit,
                                       u.LastActivityAt
                                   }).ToTable("User");
        }
    }
}