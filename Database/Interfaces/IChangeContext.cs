using Library.Classes;

namespace Database.Interfaces
{
    public interface IChangeContext
    {
        /// <summary>
        /// Changes a balance in the database.
        /// </summary>
        /// <param name="balance">The balance to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        void ChangeBalance(Balance balance, string password);

        /// <summary>
        /// Changes a payment in the database.
        /// </summary>
        /// <param name="payment">The payment to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        void ChangePayment(Payment payment, string password);

        /// <summary>
        /// Changes a transaction in the database.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        void ChangeTransaction(Transaction transaction, string password);

        /// <summary>
        /// Changes everything but the password of a user in the database.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="currencyId">The id of the prefferred currency of the user.</param>
        /// <param name="languageId">The id of the prefferred language of the user.</param>
        void ChangeUser(string name, string lastName, string email, int currencyId, int languageId, string password, string salt);

        /// <summary>
        /// Changes a password of a user in the database.
        /// </summary>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="newPassword">The new password of the user.</param>
        void ChangePassword(string email, string newPassword);
    }
}