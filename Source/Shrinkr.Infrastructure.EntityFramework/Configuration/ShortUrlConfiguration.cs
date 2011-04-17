namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations;

    using DomainObjects;

    public class ShortUrlConfiguration : EntityTypeConfiguration<ShortUrl>
    {
        public ShortUrlConfiguration()
        {
            HasKey(s => s.Id);

            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.Url).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(2048);
            Property(s => s.Domain).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);
            Property(s => s.Hash).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(24);
            Property(s => s.Title).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(2048);
            Property(s => s.InternalSpamStatus).HasColumnName("SpamStatus");

            ToTable("ShortUrl");
        }
    }
}