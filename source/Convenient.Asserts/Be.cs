using System;
using System.Linq.Expressions;

namespace Convenient.Asserts
{
    public class Be
    {
        public static Expression<Func<T, bool>> EqualTo<T>(T item)
        {
            return t => t.Equals(item);
        }
    }
}