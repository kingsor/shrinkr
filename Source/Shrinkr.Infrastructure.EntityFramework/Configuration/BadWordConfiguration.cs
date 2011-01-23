namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class BadWordConfiguration : EntityTypeConfiguration<BadWord>
    {
        public BadWordConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);
            Property(b => b.Expression).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            ToTable("BadWord");
        }
    }
}