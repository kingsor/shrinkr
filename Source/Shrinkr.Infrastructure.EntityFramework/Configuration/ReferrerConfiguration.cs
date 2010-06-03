namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class ReferrerConfiguration : ComplexTypeConfiguration<Referrer>
    {
        public ReferrerConfiguration()
        {
            Property(r => r.Domain).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(440);
            Property(r => r.Url).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(2048);
        }
    }
}