using System;
using Convenient.Asserts;
using Conventient.UnitTests.TestData;
using NUnit.Framework;
using Throws = Convenient.Asserts.Throws;

namespace Conventient.UnitTests.Asserts
{
    [TestFixture]
    public class VerifyTest
    {
        [Test]
        public void Verify_That_GetsMessageFromExpression()
        {
            var a = new TestObject();

            Verify.That(() => Verify.That(a, o => o.InnerObjects == null),
                Throws.A<Exception>().With(e => e.Message.EndsWith("TestObject does not have InnerObjects == null")));
        }
    }
}