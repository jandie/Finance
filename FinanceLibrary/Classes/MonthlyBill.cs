using FinanceLibrary.Enums;

namespace FinanceLibrary.Classes
{
    public class MonthlyBill : Payment, IPayment
    {
        /// <summary>
        /// Creates an instance of MonthlyBill.
        /// </summary>
        /// <param name="id">The ID of the Bill.</param>
        /// <param name="name">The name of the Bill.</param>
        /// <param name="amount">The amount of the Bill.</param>
        /// <param name="paymentType">The paymentType of the Bill.</param>
        public MonthlyBill(int id, string name, decimal amount, PaymentType paymentType)
            : base(id, name, amount, paymentType)
        {
        }

        /// <summary>
        /// Calculates the total sum of all transactions of the Bill.
        /// </summary>
        /// <returns>The total sum.</returns>
        public decimal GetSum()
        {
            decimal toPay = 0;

            Transactions.ForEach(t => toPay += t.Amount);

            toPay -= Amount;

            if (toPay > 0) toPay = 0;

            return toPay;
        }

        /// <summary>
        /// Calculates the total amount paid.
        /// </summary>
        /// <returns>The total amount paid.</returns>
        public decimal GetTotalAmount()
        {
            decimal paid = 0;

            Transactions.ForEach(t => paid += t.Amount);

            return paid;
        }
    }
}