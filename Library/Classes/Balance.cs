namespace Library.Classes
{
    public class Balance
    {
        public Balance(int id, string name, decimal balanceAmount)
        {
            Id = id;
            Name = name;
            BalanceAmount = balanceAmount;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal BalanceAmount { get; set; }

        public string NameSalt { get; set; }

        public string BalanceAmountSalt { get; set; }
    }
}