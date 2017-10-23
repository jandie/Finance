using System;

namespace FinanceLibrary.Exceptions
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
