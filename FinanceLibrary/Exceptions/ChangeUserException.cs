using System;

namespace FinanceLibrary.Exceptions
{
    public class ChangeUserException : Exception
    {
        public ChangeUserException()
        {
            
        }

        public ChangeUserException(string message) : base(message)
        {
            
        }
    }
}
