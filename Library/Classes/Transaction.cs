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
            AmountSalt = null;
            DescriptionSalt = null;
        }

        public Transaction(int id, decimal amount, string description, bool positive, string amountSalt, string descriptionSalt)
        {
            Id = id;
            Amount = amount;
            Description = description;
            Positive = positive;
            AmountSalt = amountSalt;
            DescriptionSalt = descriptionSalt;
        }

        public int Id { get; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string AmountSalt { get; set; }

        public string DescriptionSalt { get; set; }

        public bool Positive { get; }

        public int CompareTo(Transaction other)
        {
            return other.Id.CompareTo(Id);
        }
    }
}