using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Exceptions
{
    public class DeleteTransactionException : Exception
    {
        public DeleteTransactionException()
        {
            
        }

        public DeleteTransactionException(string message) : base(message)
        {
            
        }
    }
}
