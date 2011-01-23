namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class BannedDomainConfiguration : EntityTypeConfiguration<BannedDomain>
    {
        public BannedDomainConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);
            Property(b => b.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            ToTable("BannedDomain");
        }
    }
}