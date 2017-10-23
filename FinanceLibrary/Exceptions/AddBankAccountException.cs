using System;

namespace FinanceLibrary.Exceptions
{
    public class AddBankAccountException : Exception
    {
        public AddBankAccountException()
        {
            
        }

        public AddBankAccountException(string message) : base(message)
        {
            
        }
    }
}
