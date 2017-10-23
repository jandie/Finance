using System;
using FinanceLibrary.Enums;

namespace FinanceLibrary.Classes
{
    public class MonthlyIncome : Payment, IPayment
    {
        /// <summary>
        /// Creates an instance of MonthlyIncome.
        /// </summary>
        /// <param name="id">The ID of the Income.</param>
        /// <param name="name">The name of the Income.</param>
        /// <param name="amount">The amount of the Income.</param>
        /// <param name="paymentType">The paymentType of the Income.</param>
        public MonthlyIncome(int id, string name, decimal amount, PaymentType paymentType)
            : base(id, name, amount, paymentType)
        {
        }

        /// <summary>
        /// Calculates the total sum of all transactions of the Income.
        /// </summary>
        /// <returns>The total sum.</returns>
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

        /// <summary>
        /// Calculates the total amount paid.
        /// </summary>
        /// <returns>The total amount paid.</returns>
        public decimal GetTotalAmount()
        {
            decimal gotten = 0;

            Transactions.ForEach(t => gotten += t.Amount);

            return gotten;
        }
    }
}