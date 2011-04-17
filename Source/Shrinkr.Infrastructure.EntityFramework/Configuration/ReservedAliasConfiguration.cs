namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class ReservedAliasConfiguration : EntityTypeConfiguration<ReservedAlias>
    {
        public ReservedAliasConfiguration()
        {
            HasKey(r => r.Id);

            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            ToTable("ReservedAlias");
        }
    }
}