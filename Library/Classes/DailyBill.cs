using Library.Interfaces;

namespace Library.Classes
{
    public class DailyBill : Payment, IPayment
    {
        public bool MayAddPayment => true;

        public DailyBill(int id, string name, decimal amount) : base(id, name, amount)
        {
            
        }

        public decimal GetSum()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            if (paid >= Amount)
            {
                return (Amount - paid);
            }

            return (Amount - paid) * -1;
        }
    }
}
