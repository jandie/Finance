using System;

namespace FinanceLibrary.Exceptions
{
    public class WrongUsernameOrPasswordException : Exception
    {
        public WrongUsernameOrPasswordException()
        {
        }

        public WrongUsernameOrPasswordException(string message) : base(message)
        {
        }
    }
}