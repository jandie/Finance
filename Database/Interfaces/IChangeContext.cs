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
        /// <param name="user">The changed user to save.</param>
        void ChangeUser(User user);

        /// <summary>
        /// Changes a password of a user in the database.
        /// </summary>
        /// <param name="user">The user itself.</param>
        /// <param name="newPassword">The new password of the user.</param>
        void ChangePassword(User user, string newPassword);
    }
}