using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IChangeContext
    {
        void ChangeBalance(int id, string name, decimal balanceAmount);

        void ChangePayment(int id, string name, decimal amount);

        void ChangeTransaction(int id, decimal amount, string description);
    }
}
