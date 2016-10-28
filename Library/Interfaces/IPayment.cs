using System.Collections.Generic;
using Library.Classes;
using Library.Enums;

namespace Library.Interfaces
{
    public interface IPayment
    {
        int Id { get; set; }

        string Name { get; set; }

        decimal Amount { get; set; }

        PaymentType PaymentType { get; }

        List<Transaction> AllTransactions { get; }

        void AddTransaction(Transaction transaction);

        decimal GetSum();

        decimal GetTotalAmount();
    }
}