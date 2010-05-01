namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class AliasConfiguration : EntityConfiguration<Alias>
    {
        public AliasConfiguration()
        {
            HasKey(a => a.Id);

            Property(a => a.Id).IsIdentity();
            Property(a => a.Name).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(a => a.IPAddress).IsNotUnicode().IsRequired().IsVariableLength().HasMaxLength(15);
            Property(a => a.CreatedAt);
            Property(a => a.CreatedByApi);

            Relationship(a => a.ShortUrl).FromProperty(s => s.Aliases).IsRequired();
            Relationship(a => a.User).FromProperty(u => u.Aliases).IsOptional();

            MapSingleType(a => new
                                   {
                                       a.Id,
                                       a.Name,
                                       ShortUrlId = a.ShortUrl.Id,
                                       UserId = a.User.Id,
                                       a.IPAddress,
                                       a.CreatedAt,
                                       a.CreatedByApi
                                   }).ToTable("Alias");
        }
    }
}