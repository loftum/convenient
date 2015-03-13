using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Convenient.Asserts.Visitors
{
    public class ParameterSwapper : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> _parameterNames = new Dictionary<ParameterExpression, ParameterExpression>();

        public LambdaExpression Result { get; private set; }

        public ParameterSwapper(LambdaExpression lambda, params ParameterExpression[] newParameterNames)
        {
            if (lambda.Parameters.Count != newParameterNames.Length)
            {
                throw new ArgumentException(string.Format("Parameter count mismatch. Exising: {0}, New parameters: {1}", lambda.Parameters.Count, newParameterNames.Length));
            }
            var ii = 0;
            foreach (var parameter in lambda.Parameters)
            {
                _parameterNames[parameter] = newParameterNames[ii];
                ii++;
            }
            Result = (LambdaExpression)Visit(lambda);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterNames.ContainsKey(node))
            {
                return _parameterNames[node];
            }
            return base.VisitParameter(node);
        }
    }
}