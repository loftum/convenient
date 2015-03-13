using System;
using System.Linq.Expressions;
using Convenient.Asserts;
using Convenient.Expressions.Extensions;
using Conventient.UnitTests.TestData;
using NUnit.Framework;


namespace Conventient.UnitTests.Expressions
{
    [TestFixture]
    public class LambdaTests
    {
        [Test]
        public void GetMemberPath_ReturnsSimpleProperty()
        {
            Expression<Func<TestObject, object>> expression = o => o.Number;
            Verify.That(expression.GetMemberPath(), Be.EqualTo("Number"));
        }

        [Test]
        public void GetMemberPath_ReturnsPath()
        {
            Expression<Func<TestObject, object>> expression = o => o.Inner.String;
            Verify.That(expression.GetMemberPath(), Be.EqualTo("Inner.String"));
        }
    }
}