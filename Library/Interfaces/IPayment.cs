using System.Collections.Generic;
using Library.Classes;
using Library.Enums;

namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        string Name { get; }

        decimal Amount { get; }

        PaymentType PaymentType { get; }

        List<Transaction> AllTransactions { get; }

        void AddTransactions(List<Transaction> transactions);

        decimal GetSum();

        decimal GetTotalAmount();
    }
}