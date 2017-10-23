using System.Collections.Generic;
using FinanceLibrary.Enums;

namespace FinanceLibrary.Classes
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