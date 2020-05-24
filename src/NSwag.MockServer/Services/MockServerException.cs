using System;

namespace NSwag.MockServer
{
    public class MockServerException : Exception
    {
        protected MockServerException(string message) : base(message)
        {
        }
    }
}