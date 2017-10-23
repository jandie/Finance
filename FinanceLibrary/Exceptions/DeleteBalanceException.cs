using System;

namespace FinanceLibrary.Exceptions
{
    public class DeleteBalanceException : Exception
    {
        public DeleteBalanceException()
        {
            
        }

        public DeleteBalanceException(string message) : base (message)
        {
            
        }
    }
}
