using System;

namespace Library.Exceptions
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
