using Library.Interfaces;

namespace Library.Classes
{
    public class MonthlyBill : Payment, IPayment
    {
        public bool MayAddPayment => Transactions.Count == 0;

        public MonthlyBill(int id, string name, decimal amount) : base(id, name, amount)
        {
            
        }

        public decimal GetSum()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            if (paid >= Amount) return 0;

            return -Amount;
        }
    }
}
