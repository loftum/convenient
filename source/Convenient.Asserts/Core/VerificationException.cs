using System;

namespace Convenient.Asserts.Core
{
    public class VerificationException : Exception
    {
        public VerificationException(string message) : base(message)
        {
        }
    }
}