using Library.Enums;
using Library.Interfaces;

namespace Library.Classes
{
    public class MonthlyBill : Payment, IPayment
    {
        public MonthlyBill(int id, string name, decimal amount, PaymentType paymentType)
            : base(id, name, amount, paymentType)
        {
        }

        public decimal GetSum()
        {
            decimal toPay = 0;

            Transactions.ForEach(t => toPay += t.Amount);

            toPay -= Amount;

            if (toPay > 0) toPay = 0;

            return toPay;
        }

        public decimal GetTotalAmount()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            return paid;
        }
    }
}