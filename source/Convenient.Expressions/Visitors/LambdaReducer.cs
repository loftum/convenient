using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Convenient.Stuff;

namespace Convenient.Expressions.Visitors
{
    public class LambdaReducer : ExpressionVisitor
    {
        private readonly IList<ParameterExpression> _parameters;
        public Expression Expression { get; private set; }

        public LambdaReducer(LambdaExpression lambda)
        {
            _parameters = lambda.Parameters;
            Expression = Visit(lambda);
        }

        public sealed override Expression Visit(Expression node)
        {
            if (node == null)
            {
                return null;
            }
            if (CanRetrieveValueFrom(node))
            {
                var value = GetValueFrom(node);
                return value;
            }
            return base.Visit(node);
        }

        private bool CanRetrieveValueFrom(Expression expression)
        {
            var value = new TypeSwitch<bool>()
                .Case<ConstantExpression>(c => true)
                .Case<UnaryExpression>(c => CanRetrieveValueFrom(c.Operand))
                .Case<NewExpression>(c => !c.Arguments.Any() || c.Arguments.All(CanRetrieveValueFrom))
                .Case<MethodCallExpression>(CanRetrieveValueFrom)
                .Case<MemberExpression>(m => CanRetrieveValueFrom(m.Expression) && (m.Member is FieldInfo || m.Member is PropertyInfo))
                .Case<BinaryExpression>(b => CanRetrieveValueFrom(b.Left) && CanRetrieveValueFrom(b.Right))
                .Case<ParameterExpression>(p => _parameters.All(e => e.Name != p.Name))
                .Case<LambdaExpression>(l => l.Parameters.All(CanRetrieveValueFrom))
                .Default(false)
                .Execute(expression);
            return value;
        }

        private bool CanRetrieveValueFrom(MethodCallExpression methodCall)
        {
            var value = (methodCall.Method.IsStatic || CanRetrieveValueFrom(methodCall.Object)) &&
                (!methodCall.Arguments.Any() || methodCall.Arguments.All(CanRetrieveValueFrom));
            return value;
        }

        private static ConstantExpression GetValueFrom(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            var value = lambda.Compile().DynamicInvoke();
            return Expression.Constant(value);
        }
    }
}