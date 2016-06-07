using System;

namespace Library.Exceptions
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
