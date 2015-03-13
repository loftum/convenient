using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Convenient.Asserts;
using Convenient.Expressions.Visitors;
using Conventient.UnitTests.TestData;
using NUnit.Framework;

namespace Conventient.UnitTests.Asserts
{
    [TestFixture]
    public class LambdaReducerTest
    {
        [Test]
        public void LambdaReducer_GetsValuesFromHolder()
        {
            var list = new ValueHolder<IList<string>>(new[] {"agurk", "banan", "eple"});
            Expression<Func<TestObject, bool>> lambda = o => o.String == list.Value.Skip(1).First();

            var expression = new LambdaReducer(lambda).Expression;
            Verify.That(expression.ToString(), Be.EqualTo("o => (o.String == \"banan\")"));
        }
    }
}