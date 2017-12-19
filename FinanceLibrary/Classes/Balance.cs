using Newtonsoft.Json;

namespace FinanceLibrary.Classes
{
    public class Balance
    {
        /// <summary>
        /// Creates new instance of the Balance object.
        /// </summary>
        /// <param name="id">The ID of the balance.</param>
        /// <param name="name">The name of the Balance.</param>
        /// <param name="balanceAmount">The amount of the Balance.</param>
        public Balance(int id, string name, decimal balanceAmount)
        {
            Id = id;
            Name = name;
            BalanceAmount = balanceAmount;
        }

        /// <summary>
        /// The ID of the balance.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the Balance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The amount of the Balance.
        /// </summary>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Salt for the name of the balance. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string NameSalt { get; set; }

        /// <summary>
        /// Salt for the amount of the balance. Used for encryption.
        /// </summary>
        [JsonIgnore]
        public string BalanceAmountSalt { get; set; }
    }
}