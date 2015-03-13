using System.Linq.Expressions;

namespace Convenient.Asserts.Visitors
{
    public class OperatorMap
    {
        public static string Map(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "OR";
                default:
                    return type.ToString();
            }
        }
    }
}