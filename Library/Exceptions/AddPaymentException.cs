using System;

namespace Library.Exceptions
{
    public class AddPaymentException : Exception
    {
        public AddPaymentException()
        {

        }

        public AddPaymentException(string message) : base(message)
        {

        }
    }
}
