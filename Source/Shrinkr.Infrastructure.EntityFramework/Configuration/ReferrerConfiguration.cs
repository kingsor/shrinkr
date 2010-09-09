namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class ReferrerConfiguration : ComplexTypeConfiguration<Referrer>
    {
        public ReferrerConfiguration()
        {
            Property(r => r.Domain).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(r => r.Url).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(2048);
        }
    }
}