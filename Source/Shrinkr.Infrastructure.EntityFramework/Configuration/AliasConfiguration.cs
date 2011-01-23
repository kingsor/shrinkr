namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class AliasConfiguration : EntityTypeConfiguration<Alias>
    {
        public AliasConfiguration()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);
            Property(a => a.Name).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(a => a.IPAddress).IsUnicode(false).IsRequired().IsVariableLength().HasMaxLength(15);
            Property(a => a.CreatedAt);
            Property(a => a.CreatedByApi);

            HasRequired(a => a.ShortUrl).WithMany(s => s.Aliases);
            HasOptional(a => a.User).WithMany(u => u.Aliases);

            ToTable("Alias");
        }
    }
}