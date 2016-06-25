using System;
using Library.Interfaces;

namespace Library.Classes
{
    public class MonthlyIncome : Payment, IPayment
    {
        public bool MayAddPayment => true;

        public MonthlyIncome(int id, string name, decimal amount) : base(id, name, amount)
        {
            
        }

        public decimal GetSum()
        {
            decimal gotten = 0;

            Transactions.ForEach(t => gotten += t.Amount);

            gotten -= Amount;

            if (gotten < 0) gotten = Math.Abs(gotten);

            return gotten;
        }

        public decimal GetTotalAmount()
        {
            decimal gotten = 0;

            Transactions.ForEach(t => gotten += t.Amount);

            return gotten;
        }
    }
}
