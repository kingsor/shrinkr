namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.Id);

            Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(u => u.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(256);
            Property(u => u.Email).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(256);
            Property(u => u.IsLockedOut);
            Property(u => u.CreatedAt);
            Property(u => u.InternalRole).HasColumnName("Role");
            Property(u => u.LastActivityAt);
            
            ToTable("User");
        }
    }
}