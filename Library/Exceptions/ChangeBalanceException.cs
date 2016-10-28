using System;

namespace Library.Exceptions
{
    public class ChangeBalanceException : Exception
    {
        public ChangeBalanceException()
        {
            
        }

        public ChangeBalanceException(string message) : base(message)
        {
            
        }
    }
}
