using System;

namespace FinanceLibrary.Exceptions
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
