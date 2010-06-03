namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class ShortUrlConfiguration : EntityConfiguration<ShortUrl>
    {
        public ShortUrlConfiguration()
        {
            HasKey(s => s.Id);

            Property(s => s.Id).IsIdentity();
            Property(s => s.Url).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(2048);
            Property(s => s.Domain).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);
            Property(s => s.Hash).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(24);
            Property(s => s.Title).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(2048);
            Property(s => s.InternalSpamStatus);

            MapSingleType(s => new
                                   {
                                       s.Id,
                                       s.Url,
                                       s.Domain,
                                       s.Hash,
                                       s.Title,
                                       SpamStatus = s.InternalSpamStatus
                                   }).ToTable("ShortUrl");
        }
    }
}