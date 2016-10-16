using System;

namespace Library.Exceptions
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
