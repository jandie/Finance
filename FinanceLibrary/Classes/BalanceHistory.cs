using System;
using Newtonsoft.Json;

namespace FinanceLibrary.Classes
{
    public class BalanceHistory
    {
        /// <summary>
        /// The ID of the BalanceHistory object.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The amount of the balance.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The salt of the amount. Used for encryption in the databse.
        /// </summary>
        [JsonIgnore]
        public string AmountSalt { get; set; }

        /// <summary>
        /// The time when the BalanceHistory object was created.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Creates a new instance of the BalanceHistory object.
        /// The DateTime is set to the current DateTime.
        /// </summary>
        /// <param name="amount">The amount of the balance.</param>
        public BalanceHistory(int id, decimal amount, string amountSalt)
        {
            Id = id;
            Amount = amount;
            AmountSalt = amountSalt;
            DateTime = DateTime.Now.Date;
        }
    }
}
