using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Convenient.Reflection.Extensions
{
    public static class TypeExtensions
    {
        private static readonly IDictionary<Type, string> ValueNames = new Dictionary<Type, string>
        {
            {typeof(int), "int"},
            {typeof(short), "short"},
            {typeof(byte), "byte"},
            {typeof(bool), "bool"},
            {typeof(long), "long"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"},
            {typeof(string), "string"}
        };

        public static string GetFriendlyName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (ValueNames.ContainsKey(type))
            {
                return ValueNames[type];
            }
            if (type.IsGenericType)
            {
                return string.Format("{0}<{1}>", type.Name.Split('`')[0],
                    string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)));
            }
            return type.Name;
        }

        public static bool IsSimpleType(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        public static IEnumerable<MemberInfo> GetPublicPropertiesAndFields(this Type type)
        {
            return type.GetMembers().Where(m => m is PropertyInfo || m is FieldInfo);
        }
    }
}