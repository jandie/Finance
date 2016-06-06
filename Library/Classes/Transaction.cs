namespace Library.Classes
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public Transaction(int id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
