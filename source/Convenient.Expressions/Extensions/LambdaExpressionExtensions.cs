using System;
using System.Linq.Expressions;
using System.Reflection;
using Convenient.Expressions.Visitors;

namespace Convenient.Expressions.Extensions
{
    public static class LambdaExpressionExtensions
    {
        public static PropertyInfo GetProperty(this LambdaExpression lambda)
        {
            var member = lambda.GetMember();
            var property = member as PropertyInfo;
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("{0} is a {1}, not a property", member.Name, member.MemberType));
            }
            return property;
        }

        public static FieldInfo GetField(this LambdaExpression lambda)
        {
            var member = lambda.GetMember();
            var field = member as FieldInfo;
            if (field == null)
            {
                throw new InvalidOperationException(string.Format("{0} is a {1}, not a field", member.Name, member.MemberType));
            }
            return field;
        }

        public static MemberInfo GetMember(this LambdaExpression lambda)
        {
            var memberExpression = lambda.GetMemberExpressionOrNull();
            if (memberExpression == null)
            {
                throw new InvalidOperationException(string.Format("Could not get member from {0}", lambda));
            }
            return memberExpression.Member;
        }

        public static string GetMemberPath<T>(this Expression<Func<T, object>> expression)
        {
            return new MemberPathVisitor(expression).MemberPath;
        }

        public static MemberExpression GetMemberExpressionOrNull(this LambdaExpression lambda)
        {
            return GetMemberExpression(lambda.Body);
        }

        public static Expression StripQuotes(this Expression expression)
        {
            var exp = expression;
            while (exp is UnaryExpression)
            {
                exp = ((UnaryExpression)exp).Operand;
            }
            return exp;
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            return DoGetMemberExpression((dynamic)expression);
        }

        private static MemberExpression DoGetMemberExpression(MemberExpression expression)
        {
            return expression;
        }

        private static MemberExpression DoGetMemberExpression(UnaryExpression expression)
        {
            return GetMemberExpression(expression.Operand);
        }

        private static MemberExpression DoGetMemberExpression(object invalid)
        {
            return null;
        }
    }
}