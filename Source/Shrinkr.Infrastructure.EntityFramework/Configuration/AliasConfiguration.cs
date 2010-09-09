namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

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

            HasRequired(a => a.ShortUrl).WithMany(s => s.Aliases);
            HasOptional(a => a.User).WithMany(u => u.Aliases);

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