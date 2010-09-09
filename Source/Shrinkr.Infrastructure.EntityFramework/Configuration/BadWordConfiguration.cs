namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class BadWordConfiguration : EntityConfiguration<BadWord>
    {
        public BadWordConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id).IsIdentity();
            Property(b => b.Expression).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            MapSingleType(b => new
                                   {
                                       b.Id,
                                       b.Expression
                                   }).ToTable("BadWord");
        }
    }
}