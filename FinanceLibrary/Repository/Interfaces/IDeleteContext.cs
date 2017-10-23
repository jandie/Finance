namespace FinanceLibrary.Repository.Interfaces
{
    public interface IDeleteContext
    {
        /// <summary>
        /// Deactivates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        void DeactivateUser(int id);

        /// <summary>
        /// Activates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        void ActivateUser(int id);

        /// <summary>
        /// Deletes balance from the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        void DeleteBalance(int id);

        /// <summary>
        /// Deletes payment from the database.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        void DeletePayment(int id);

        /// <summary>
        /// Deletes transaction from the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        void DeleteTransaction(int id);

        /// <summary>
        /// Deletes user from the database.
        /// </summary>
        /// <param name="email"></param>
        void DeleteUser(string email);
    }
}