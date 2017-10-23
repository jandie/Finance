using System;

namespace FinanceLibrary.Exceptions
{
    public class DeleteTransactionException : Exception
    {
        public DeleteTransactionException()
        {
            
        }

        public DeleteTransactionException(string message) : base(message)
        {
            
        }
    }
}
