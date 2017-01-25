using System;
using System.Collections.Generic;
using Library.Enums;

namespace Library.Classes
{
    public abstract class Payment : ICloneable
    {
        protected List<Transaction> Transactions;

        protected Payment(int id, string name, decimal amount, PaymentType paymentType)
        {
            Id = id;
            Amount = amount;
            PaymentType = paymentType;
            Name = name;
            NameSalt = null;
            AmountSalt = null;

            Transactions = new List<Transaction>();
        }

        protected Payment(int id, string name, decimal amount, PaymentType paymentType, string nameSalt, string amountSalt)
        {
            Id = id;
            Amount = amount;
            PaymentType = paymentType;
            Name = name;
            NameSalt = nameSalt;
            AmountSalt = amountSalt;

            Transactions = new List<Transaction>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string NameSalt { get; set; }

        public string AmountSalt { get; set; }

        public PaymentType PaymentType { get; }

        public List<Transaction> AllTransactions => new List<Transaction>(Transactions);

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void DeleteTransaction(int id)
        {
            Transactions.Remove(Transactions.Find(t => t.Id == id));
        }
    }
}