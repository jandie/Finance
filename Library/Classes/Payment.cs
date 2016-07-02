using System.Collections.Generic;
using Library.Enums;

namespace Library.Classes
{
    public abstract class Payment
    {
        protected List<Transaction> Transactions;

        protected Payment(int id, string name, decimal amount, PaymentType paymentType)
        {
            Id = id;
            Amount = amount;
            PaymentType = paymentType;
            Name = name;

            Transactions = new List<Transaction>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public PaymentType PaymentType { get; }

        public List<Transaction> AllTransactions => new List<Transaction>(Transactions);

        public void AddTransactions(List<Transaction> transactions)
        {
            Transactions = new List<Transaction>(transactions);
        }
    }
}