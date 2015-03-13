using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Convenient.Expressions.Visitors;
using Convenient.Reflection.Extensions;
using Convenient.Stuff;

namespace Convenient.Asserts.Visitors
{
    public class LambdaString
    {
        public string FriendlyString { get; private set; }

        public LambdaString(LambdaExpression lambda)
        {
            var reduced = new LambdaReducer(lambda).Expression;
            FriendlyString = Visit(reduced);
        }

        private string Visit(Expression expression)
        {
            return expression == null ? null : DoVisit((dynamic) expression);
        }

        private string DoVisit(BinaryExpression node)
        {
            var left = Visit(node.Left);
            var right = Visit(node.Right);
            switch (node.NodeType)
            {
                case ExpressionType.ArrayIndex:
                    return string.Format("{0}[{1}]", left, right);
                default:
                    return string.Join(" ", Visit(node.Left), OperatorMap.Map(node.NodeType), Visit(node.Right));
            }
        }

        private string DoVisit(MethodCallExpression node)
        {
            var subject = Visit(node.Method.IsExtensionMethod() ? node.Arguments.First() : node.Object);
            
            var arguments = node.Method.IsExtensionMethod() ? node.Arguments.Skip(1) : node.Arguments;
            return FormatMethod(subject, node.Method, arguments.Select(Visit));
        }

        private static string FormatMethod(string subject, MethodInfo method, IEnumerable<string> arguments)
        {
            if (method.IsSpecialName)
            {
                switch (method.Name)
                {
                    case "get_Item":
                        return string.Format("{0}[{1}]", subject, string.Join(", ", arguments));
                }
            }
            return JoinNotNull(".", subject, string.Format("{0}({1})", method.Name, string.Join(", ", arguments)));
        }


        private string DoVisit(MemberExpression node)
        {
            return JoinNotNull(".", Visit(node.Expression), node.Member.Name);
        }

        private static string JoinNotNull(string separator, params string[] parts)
        {
            return string.Join(separator, parts.Where(p => p != null));
        }

        private string DoVisit(UnaryExpression node)
        {
            return Visit(node.Operand);
        }

        private static string DoVisit(ConstantExpression node)
        {
            return Format(node.Value);
        }

        private string DoVisit(LambdaExpression lambda)
        {
            return Visit(lambda.Body);
        }

        private static string DoVisit(ParameterExpression parameter)
        {
            return null;
        }

        private static string DoVisit(object unknown)
        {
            return Format(unknown);
        }

        private static string Format(object value)
        {
            return TypeSwitch.On<string>(value, sw => sw
                .Case<string>(s => string.Format("\"{0}\"", s))
                .Case<char>(c => string.Format("'{0}'", c))
                .Default(value == null ? "null" : value.ToString())
                );
        }
    }
}