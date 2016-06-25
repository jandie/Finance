using System.Collections.Generic;
using Library.Classes;

namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        string Name { get; }

        decimal Amount { get; }

        List<Transaction> AllTransactions { get; }

        bool MayAddPayment { get; }

        decimal GetSum();

        decimal GetTotalAmount();

        void AddTransactions(List<Transaction> transactions);
    }
}
