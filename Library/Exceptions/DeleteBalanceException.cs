using System;

namespace Library.Exceptions
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
