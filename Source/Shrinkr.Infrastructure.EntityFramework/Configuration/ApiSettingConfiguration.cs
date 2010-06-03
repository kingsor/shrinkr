namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class ApiSettingConfiguration : ComplexTypeConfiguration<ApiSetting>
    {
        public ApiSettingConfiguration()
        {
            Property(s => s.Key).IsUnicode().IsOptional().IsVariableLength().HasMaxLength(36);
            Property(s => s.Allowed);
            Property(s => s.DailyLimit);
        }
    }
}