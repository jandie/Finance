using System;

namespace Library.Exceptions
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
