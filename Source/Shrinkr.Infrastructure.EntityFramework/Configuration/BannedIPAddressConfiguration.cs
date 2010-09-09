namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class BannedIPAddressConfiguration : EntityConfiguration<BannedIPAddress>
    {
        public BannedIPAddressConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).IsIdentity();
            Property(b => b.IPAddress).IsNotUnicode().IsRequired().IsVariableLength().HasMaxLength(15);

            MapSingleType(b => new
                                   {
                                       b.Id,
                                       b.IPAddress
                                   }).ToTable("BannedIPAddress");
        }
    }
}