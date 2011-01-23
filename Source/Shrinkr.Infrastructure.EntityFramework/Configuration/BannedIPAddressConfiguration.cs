namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class BannedIPAddressConfiguration : EntityTypeConfiguration<BannedIPAddress>
    {
        public BannedIPAddressConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);
            Property(b => b.IPAddress).IsUnicode(false).IsRequired().IsVariableLength().HasMaxLength(15);

            ToTable("BannedIPAddress");
        }
    }
}