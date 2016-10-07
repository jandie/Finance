using System;

namespace Library.Exceptions
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
