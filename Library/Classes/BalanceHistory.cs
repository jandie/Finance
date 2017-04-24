using System;

namespace Library.Classes
{
    public class BalanceHistory
    {
        public int Id { get; set; }
        public decimal Amount { get; }
        public string AmountSalt { get; set; }
        public DateTime DateTime { get; set; }
    }
}
