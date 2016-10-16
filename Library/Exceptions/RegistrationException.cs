using System;

namespace Library.Exceptions
{
    public class RegistrationException : Exception
    {
        public RegistrationException()
        {
        }

        public RegistrationException(string message) : base(message)
        {
        }
    }
}