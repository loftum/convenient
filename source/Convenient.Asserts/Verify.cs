using System;
using System.Linq.Expressions;
using Convenient.Asserts.Core;
using Convenient.Asserts.Extensions;

namespace Convenient.Asserts
{
    public class Verify
    {
        public static void That<T>(T item, Expression<Func<T, bool>> condition)
        {
            var func = condition.Compile();
            if (!func(item))
            {
                throw new VerificationException(string.Format("{0} does not have {1}", item, condition.ToFriendlyString()));
            }
        }

        public static void That<T>(T action, ICondition<T> condition)
        {
            condition.Verify(action);
        }
    }
}