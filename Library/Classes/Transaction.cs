using System;

namespace Library.Classes
{
    public class Transaction : IComparable<Transaction>
    {
        public Transaction(int id, decimal amount, string description, bool positive)
        {
            Id = id;
            Amount = amount;
            Description = description;
            Positive = positive;
        }

        public int Id { get; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public bool Positive { get; }

        public int CompareTo(Transaction other)
        {
            return other.Id.CompareTo(Id);
        }
    }
}