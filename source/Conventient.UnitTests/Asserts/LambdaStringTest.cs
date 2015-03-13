using System;
using System.Linq;
using System.Linq.Expressions;
using Convenient.Asserts;
using Convenient.Asserts.Visitors;
using Conventient.UnitTests.TestData;
using NUnit.Framework;

namespace Conventient.UnitTests.Asserts
{
    [TestFixture]
    public class LambdaStringTest
    {
        public string Value { get; set; }

        public LambdaStringTest()
        {
            Value = "pølse";
        }

        [Test]
        public void SimpleEquals()
        {
            var lambdaString = LambdaString<TestObject>(o => o.String == "pølse");
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("String == \"pølse\""));
        }

        [Test]
        public void LocalVariable()
        {
            const string value = "pølse";
            var lambdaString = LambdaString<TestObject>(o => o.String == value);
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("String == \"pølse\""));
        }

        [Test]
        public void LocalProperty()
        {
            var lambdaString = LambdaString<TestObject>(o => o.String == Value);
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("String == \"pølse\""));
        }

        [Test]
        public void GetValueFromProperty()
        {
            var holder = new ValueHolder<string>("pølse");
            var lambdaString = LambdaString<TestObject>(o => o.String == holder.Value);
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("String == \"pølse\""));
        }

        [Test]
        public void GetValueFromValueHolder()
        {
            var holder = new ValueHolder<string>("pølse");
            var lambdaString = LambdaString<TestObject>(o => o.String == holder.Value + o.StringField);
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("String == \"pølse\" + StringField"));
        }

        [Test]
        public void Should()
        {
            var value = "hest";
            var lambdaString = LambdaString<TestObject>(o => o.InnerObjects[0].String == value.FirstOrDefault(c => c == 'e').ToString());
            Verify.That(lambdaString.FriendlyString, Be.EqualTo("InnerObjects[0].String == \"e\""));
        }

        private static LambdaString LambdaString<T>(Expression<Func<T, bool>> condition)
        {
            return new LambdaString(condition);
        }
    }
}