using System.Collections.Generic;

namespace Library.Classes
{
    public abstract class Payment
    {
        protected List<Transaction> Transactions;
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public List<Transaction> AllTransactions => new List<Transaction>(Transactions);

        protected Payment(int id, string name, decimal amount)
        {
            Id = id;
            Amount = amount;
            Name = name;

            Transactions = new List<Transaction>();
        }

        public void AddTransactions(List<Transaction> transactions)
        {
            Transactions = new List<Transaction>(transactions);
        }
    }
}
