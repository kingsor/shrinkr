namespace Shrinkr.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Script.Serialization;

    using DomainObjects;
    using Microsoft.Practices.ServiceLocation;
    using MvcExtensions;

    public class CamelCasedJsonConverter : JavaScriptConverter
    {
        private static readonly Type scriptIgnoreAttributeType = typeof(ScriptIgnoreAttribute);

        private static readonly Func<IEnumerable<Type>> getSupportedTypes = () => ServiceLocator.Current
                                                                                                .GetInstance<IBuildManager>()
                                                                                                .ConcreteTypes
                                                                                                .Where(type => (type.Name.EndsWith("DTO", StringComparison.OrdinalIgnoreCase) || type.Name.EndsWith("Model", StringComparison.OrdinalIgnoreCase) || typeof(IEntity).IsAssignableFrom(type)));

        private static IEnumerable<Type> supportedTypes;

        public override IEnumerable<Type> SupportedTypes
        {
            [DebuggerStepThrough]
            get
            {
                return supportedTypes ?? (supportedTypes = getSupportedTypes().ToList());
            }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Func<string, string> camelCase = name => name.Substring(0, 1).ToLower(Culture.Invariant) + name.Substring(1);

            IDictionary<string, object> result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (obj != null)
            {
                Type type = obj.GetType();

                IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField)
                                                    .Where(field => !field.IsDefined(scriptIgnoreAttributeType, true));

                foreach (FieldInfo field in fields)
                {
                    string key = camelCase(field.Name);
                    object value = field.GetValue(obj);

                    result.Add(key, value);
                }

                Func<PropertyInfo, bool> shouldInclude = property => !property.IsDefined(scriptIgnoreAttributeType, true) &&
                                                                     (property.GetGetMethod() != null) &&
                                                                     (property.GetGetMethod().GetParameters().Length == 0);

                IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                                                           .Where(shouldInclude);

                foreach (PropertyInfo property in properties)
                {
                    string key = camelCase(property.Name);
                    object value = property.GetValue(obj, null);

                    result.Add(key, value);
                }
            }

            return result;
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}