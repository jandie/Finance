using System;

namespace FinanceLibrary.Exceptions
{
    public class ChangePaymentException : Exception
    {
        public ChangePaymentException()
        {
            
        }

        public ChangePaymentException(string message) : base(message)
        {
            
        }
    }
}
