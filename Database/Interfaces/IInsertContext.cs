using Library.Enums;

namespace Database.Interfaces
{
    public interface IInsertContext
    {
        /// <summary>
        /// Adds a balance to the databse.
        /// </summary>
        /// <param name="userId">The id of the user the balance belongs to.</param>
        /// <param name="name">The name of the balance.</param>
        /// <param name="balance">The balance of the balance.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        int AddBankAccount(int userId, string name, decimal balance, string password, string salt);

        /// <summary>
        /// Adds a payment to the database.
        /// </summary>
        /// <param name="userId">The id of the user the payment belongs to.</param>
        /// <param name="name">The name of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <param name="type">The type of the payment.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        int AddPayment(int userId, string name, decimal amount, PaymentType type, string password, string salt);

        /// <summary>
        /// Adds a transaction to the databse.
        /// </summary>
        /// <param name="paymentId">The id of the payment the transaction belongs to.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        int AddTransaction(int paymentId, decimal amount, string description, string password, string salt);
    }
}