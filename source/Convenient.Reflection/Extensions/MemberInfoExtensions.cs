using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Convenient.Reflection.Extensions
{
    public static class MemberInfoExtensions
    {
        public static Expression<Func<T, object>> ToLambda<T>(this PropertyInfo property)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var call = Expression.Call(parameter, property.GetGetMethod());
            var body = ConvertIfValueType(call);
            var lambda = Expression.Lambda<Func<T, object>>(body, parameter);
            return lambda;
        }

        public static Expression<Func<T, object>> ToLambda<T>(this FieldInfo field)
        {
            var parameter = Expression.Parameter(typeof(T), "item");
            var call = Expression.Field(parameter, field);
            var body = ConvertIfValueType(call);
            return Expression.Lambda<Func<T, object>>(body, parameter);
        }

        private static Expression ConvertIfValueType(Expression expression)
        {
            return expression.Type.IsValueType ? Expression.Convert(expression, typeof(object)) : expression;
        }

        public static bool BelongsTo(this MemberInfo member, Type type)
        {
            return member.DeclaringType != null &&
                (member.DeclaringType == type || member.DeclaringType.IsAssignableFrom(type));
        }
    }
}