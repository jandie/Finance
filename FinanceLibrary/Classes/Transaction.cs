using System;
using Newtonsoft.Json;

namespace FinanceLibrary.Classes
{
    public class Transaction : IComparable<Transaction>
    {
        /// <summary>
        /// Creates an instance of the Transaction object.
        /// </summary>
        /// <param name="id">The ID of the Transaction.</param>
        /// <param name="amount">The amount of the Transaction.</param>
        /// <param name="description">The description of the Transaction.</param>
        /// <param name="positive">Whether or not the Transaction is positive.</param>
        public Transaction(int id, decimal amount, string description, bool positive)
        {
            Id = id;
            Amount = amount;
            Description = description;
            Positive = positive;
            AmountSalt = null;
            DescriptionSalt = null;
        }

        /// <summary>
        /// The ID of the Transaction.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The amount of the Transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The description of the Transaction.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The salt of the amount. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string AmountSalt { get; set; }

        /// <summary>
        /// The salt of the description. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string DescriptionSalt { get; set; }

        /// <summary>
        /// Whether or not the Transaction is positive.
        /// </summary>
        public bool Positive { get; }

        /// <summary>
        /// Compares the Transaction to another Transaction.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Transaction other)
        {
            return other.Id.CompareTo(Id);
        }
    }
}