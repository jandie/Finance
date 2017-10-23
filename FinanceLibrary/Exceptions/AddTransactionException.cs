using System;

namespace FinanceLibrary.Exceptions
{
    public class AddTransactionException : Exception
    {
        public AddTransactionException()
        {

        }

        public AddTransactionException(string message) : base(message)
        {

        }
    }
}
