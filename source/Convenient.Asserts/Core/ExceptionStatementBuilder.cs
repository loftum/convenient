using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Convenient.Asserts.Extensions;
using Convenient.Reflection.Extensions;

namespace Convenient.Asserts.Core
{
    public class ExceptionStatementBuilder<TException> : ICondition<Action> where TException : Exception
    {
        private readonly IList<Expression> _conditions = new List<Expression>();

        private readonly ParameterExpression _parameter = Expression.Parameter(typeof (TException), "ex");

        public Expression<Func<TException, bool>> GetCondition()
        {
            return Expression.Lambda<Func<TException, bool>>(GetBody(), _parameter);
        }

        private Expression GetBody()
        {
            return !_conditions.Any()
                ? Expression.Constant(true)
                : _conditions.Aggregate(Expression.AndAlso);
        }

        public ExceptionStatementBuilder<TException> With(Expression<Func<TException, bool>> condition)
        {
            _conditions.Add(condition.SwapParameter(_parameter).Body);
            return this;
        }

        public static implicit operator Expression<Func<TException, bool>>(ExceptionStatementBuilder<TException> builder)
        {
            return builder.GetCondition();
        }

        public void Verify(Action item)
        {
            try
            {
                item();
            }
            catch (TException expected)
            {
                var condition = GetCondition();
                if (!condition.Compile()(expected))
                {
                    var builder = new StringBuilder().AppendLine()
                        .AppendFormat("Expected: {0} where {1}", typeof (TException), condition.ToFriendlyString())
                        .AppendLine()
                        .AppendFormat("Actual: {0}", expected);

                    throw new VerificationException(builder.ToString());
                }
            }
            catch (Exception unexpected)
            {
                throw new VerificationException(string.Format("Epected {0} but got {1}", typeof(TException).Name, unexpected.GetType().GetFriendlyName()));
            }
        }
    }
}