using System;
namespace FinanceLibrary.Exceptions
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
