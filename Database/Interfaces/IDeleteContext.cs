namespace Database.Interfaces
{
    public interface IDeleteContext
    {
        /// <summary>
        /// Deactivates a balance in the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        void DeleteBalance(int id);

        /// <summary>
        /// Deactivates a payment in the database.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        void DeletePayment(int id);

        /// <summary>
        /// Deactivates a transaction in the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        void DeleteTransaction(int id);
    }
}