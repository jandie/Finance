namespace Library.Classes
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public Transaction(int id, decimal amount, string description)
        {
            Id = id;
            Amount = amount;
            Description = description;
        }
    }
}
