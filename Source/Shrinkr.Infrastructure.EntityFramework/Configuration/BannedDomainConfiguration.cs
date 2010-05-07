namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class BannedDomainConfiguration : EntityConfiguration<BannedDomain>
    {
        public BannedDomainConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).IsIdentity();
            Property(b => b.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            MapSingleType(b => new
                                   {
                                       b.Id,
                                       b.Name
                                   }).ToTable("BannedDomain");
        }
    }
}