namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class VisitConfiguration : EntityTypeConfiguration<Visit>
    {
        public VisitConfiguration()
        {
            HasKey(v => v.Id);

            Property(v => v.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(v => v.IPAddress).IsUnicode(false).IsRequired().IsVariableLength().HasMaxLength(15);
            Property(v => v.Browser).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(v => v.GeoCode);
            Property(v => v.CreatedAt);

            HasRequired(v => v.Alias).WithMany(a => a.Visits).Map(m => m.MapKey("AliasId"));

            ToTable("Visit");
        }
    }
}