using System;

namespace FinanceLibrary.Exceptions
{
    public class ChangePasswordException : Exception
    {
        public ChangePasswordException()
        {
            
        }

        public ChangePasswordException(string message) : base(message)
        {
            
        }
    }
}
