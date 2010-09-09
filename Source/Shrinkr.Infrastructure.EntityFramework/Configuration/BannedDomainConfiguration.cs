namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

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