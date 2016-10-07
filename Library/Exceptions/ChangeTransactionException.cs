using System;

namespace Library.Exceptions
{
    public class ChangeTransactionException : Exception
    {
        public ChangeTransactionException()
        {
            
        }

        public ChangeTransactionException(string message) : base(message)
        {
            
        }
    }
}
