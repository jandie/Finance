using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
