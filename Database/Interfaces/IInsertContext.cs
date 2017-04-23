using Library.Classes;

namespace Database.Interfaces
{
    public interface IInsertContext
    {
        /// <summary>
        /// Adds a balance to the databse.
        /// </summary>
        /// <param name="userId">The id of the user the balance belongs to.</param>
        /// <param name="balance">The new balance.</param>
        /// <param name="password">The password used of decrypting data.</param>
        int AddBankAccount(int userId, Balance balance, string password);

        /// <summary>
        /// Adds a payment to the database.
        /// </summary>
        /// <param name="userId">The id of the user the payment belongs to.</param>
        /// <param name="payment">The new payment.</param>
        /// <param name="password">The password used of decrypting data.</param>
        int AddPayment(int userId, Payment payment, string password);

        /// <summary>
        /// Adds a transaction to the databse.
        /// </summary>
        /// <param name="paymentId">The id of the payment the transaction belongs to.</param>
        /// <param name="transaction">The new transaction.</param>
        /// <param name="password">The password used of decrypting data.</param>
        int AddTransaction(int paymentId, Transaction transaction, string password);
    }
}