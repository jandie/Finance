using System;
using Library.Enums;
using Library.Interfaces;

namespace Library.Classes
{
    public class MonthlyIncome : Payment, IPayment
    {
        public MonthlyIncome(int id, string name, decimal amount, PaymentType paymentType) : base(id, name, amount, paymentType)
        {
            
        }

        public decimal GetSum()
        {
            decimal gotten = 0;

            Transactions.ForEach(t => gotten += t.Amount);

            gotten -= Amount;

            if (gotten < 0)
                gotten = Math.Abs(gotten);
            else if (gotten > 0)
                gotten = 0;

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
