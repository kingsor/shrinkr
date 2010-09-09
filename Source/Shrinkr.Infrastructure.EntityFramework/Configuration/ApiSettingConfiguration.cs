namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using DomainObjects;
    
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