using Library.Interfaces;

namespace Library.Classes
{
    public class ReocurringIncome : Payment, IPayment
    {
        public ReocurringIncome(int id, string name, decimal amount) : base(id, name, amount)
        {
            
        }

        public decimal GetSum()
        {
            decimal gotten = 0;

            Transactions.ForEach(t => gotten += t.Amount);

            if (gotten >= Amount) return 0;

            return Amount;
        }
    }
}
