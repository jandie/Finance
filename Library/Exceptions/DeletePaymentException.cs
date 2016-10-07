using System;
namespace Library.Exceptions
{
    public class DeletePaymentException : Exception
    {
        public DeletePaymentException()
        {
            
        }

        public DeletePaymentException(string message) : base(message)
        {
            
        }
    }
}
