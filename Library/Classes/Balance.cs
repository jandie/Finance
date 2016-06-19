namespace Library.Classes
{
    public class Balance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BalanceAmount { get; set; }

        public Balance(int id, string name, decimal balanceAmount)
        {
            Id = id;
            Name = name;
            BalanceAmount = balanceAmount;
        }
    }
}
