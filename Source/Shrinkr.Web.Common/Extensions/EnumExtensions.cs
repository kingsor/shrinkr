namespace Shrinkr.Web
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        [CLSCompliant(false)]
        public static string AsDescriptiveText<TEnum>(this TEnum instance) where TEnum : IComparable, IFormattable, IConvertible
        {
            FieldInfo field = instance.GetType().GetField(instance.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                      .Cast<DescriptionAttribute>()
                                                      .FirstOrDefault();

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return instance.ToString(Culture.Current);
        }
    }
}