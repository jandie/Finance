using System;

namespace FinanceLibrary.Exceptions
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
