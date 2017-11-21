using System;
using System.Collections.Generic;
using FinanceLibrary.Enums;
using Newtonsoft.Json;

namespace FinanceLibrary.Classes
{
    public abstract class Payment : ICloneable
    {
        /// <summary>
        /// List of transactions that belong to the Payment.
        /// </summary>
        protected readonly List<Transaction> Transactions;

        /// <summary>
        /// Creates an instance of the Payment object.
        /// </summary>
        /// <param name="id">The ID of the Payment.</param>
        /// <param name="name">The name of the Payment.</param>
        /// <param name="amount">The amount of the Payment.</param>
        /// <param name="paymentType">The type of Payment.</param>
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

        /// <summary>
        /// The ID of the Payment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the Payment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The amount of the Payment.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The salt of the name. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string NameSalt { get; set; }

        /// <summary>
        /// The slat of the amount. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string AmountSalt { get; set; }

        /// <summary>
        /// The type of Payment.
        /// </summary>
        public PaymentType PaymentType { get; }

        /// <summary>
        /// Gets a copy of the Transaction list.
        /// </summary>
        public List<Transaction> AllTransactions => new List<Transaction>(Transactions);

        /// <summary>
        /// Clones the current Payment.
        /// </summary>
        /// <returns>A clone of the current Payment.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Adds a Transaction to the Payment.
        /// </summary>
        /// <param name="transaction">The Transaction to add.</param>
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        /// <summary>
        /// Deletes a Transaction from the Payment.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTransaction(int id)
        {
            Transactions.Remove(Transactions.Find(t => t.Id == id));
        }
    }
}