using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
