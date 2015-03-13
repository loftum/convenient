using System;
using Convenient.Asserts.Core;

namespace Convenient.Asserts
{
    public class Throws
    {
        public static ExceptionStatementBuilder<TException> A<TException>() where TException : Exception
        {
            return new ExceptionStatementBuilder<TException>();
        }
    }
}