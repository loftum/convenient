using System.Linq.Expressions;
using Convenient.Asserts.Visitors;

namespace Convenient.Asserts.Extensions
{
    public static class LambdaExpressionExtensions
    {
        public static LambdaExpression SwapParameter(this LambdaExpression lambda, ParameterExpression parameter)
        {
            return new ParameterSwapper(lambda, parameter).Result;
        }

        public static string ToFriendlyString(this LambdaExpression lambda)
        {
            return new LambdaString(lambda).FriendlyString;
        }
    }
}