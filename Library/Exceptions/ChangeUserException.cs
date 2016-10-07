using System;

namespace Library.Exceptions
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
