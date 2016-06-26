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

        public int Id { get; }

        public string Name { get; }

        public decimal BalanceAmount { get;}
    }
}
