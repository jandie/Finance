using System;

namespace Repository.Exceptions
{
    public class UserChangeException : Exception
    {
        public UserChangeException()
        {
            
        }

        public UserChangeException(string message) : base(message)
        {
            
        }
    }
}
