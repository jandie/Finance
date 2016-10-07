namespace Database.Interfaces
{
    public interface IChangeContext
    {
        /// <summary>
        /// Changes a balance in the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        /// <param name="name">The name of the balance.</param>
        /// <param name="balanceAmount">The amount of the balance.</param>
        void ChangeBalance(int id, string name, decimal balanceAmount);

        /// <summary>
        /// Changes a payment in the databse.
        /// </summary>
        /// <param name="id">The id of the payment</param>
        /// <param name="name">The name of the payment</param>
        /// <param name="amount">The amount of the payment.</param>
        void ChangePayment(int id, string name, decimal amount);

        /// <summary>
        /// Changes a transaction in the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        void ChangeTransaction(int id, decimal amount, string description);

        /// <summary>
        /// Changes everything but the password of a user in the database.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="currencyId">The id of the prefferred currency of the user.</param>
        /// <param name="languageId">The id of the prefferred language of the user.</param>
        void ChangeUser(string name, string lastName, string email, int currencyId, int languageId);

        /// <summary>
        /// Changes a password of a user in the database.
        /// </summary>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="newPassword">The new password of the user.</param>
        void ChangePassword(string email, string newPassword);
    }
}