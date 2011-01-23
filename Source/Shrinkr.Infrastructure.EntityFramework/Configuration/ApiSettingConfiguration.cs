namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;

    public class ApiSettingConfiguration : ComplexTypeConfiguration<ApiSetting>
    {
        public ApiSettingConfiguration()
        {
            Property(s => s.Key).IsUnicode()
                                .IsOptional()
                                .IsVariableLength()
                                .HasMaxLength(36)
                                .HasColumnName("ApiKey");
            Property(s => s.Allowed).HasColumnName("ApiAllowed");
            Property(s => s.DailyLimit);
        }
    }
}