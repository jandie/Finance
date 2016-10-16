using System;

namespace Library.Exceptions
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
