namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class VisitConfiguration : EntityConfiguration<Visit>
    {
        public VisitConfiguration()
        {
            HasKey(v => v.Id);

            Property(v => v.Id).IsIdentity();
            Property(v => v.IPAddress).IsNotUnicode().IsRequired().IsVariableLength().HasMaxLength(15);
            Property(v => v.Browser).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(v => v.GeoCode);
            Property(v => v.CreatedAt);

            HasRequired(v => v.Alias).WithMany(a => a.Visits);

            MapSingleType(v => new
                                   {
                                       v.Id,
                                       AliasId = v.Alias.Id,
                                       v.IPAddress,
                                       v.Browser,
                                       ReferrerDomain = v.Referrer.Domain,
                                       ReferrerUrl = v.Referrer.Url,
                                       v.GeoCode,
                                       v.CreatedAt
                                   }).ToTable("Visit");
        }
    }
}