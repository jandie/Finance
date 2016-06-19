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

            return gotten;
        }
    }
}
