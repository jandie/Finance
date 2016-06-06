using System.Collections.Generic;

namespace Library.Classes
{
    abstract class Payment
    {
        protected List<Transaction> Transactions;
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        protected Payment(int id, string name, decimal amount)
        {
            Id = id;
            Amount = amount;
            Name = name;
            Transactions = new List<Transaction>();
        }

        public void AddTransaction(int id, decimal amount)
        {
            Transactions.Add(new Transaction(id, amount));
        }
    }
}
