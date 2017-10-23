using System;
using FinanceLibrary.Classes;
using FinanceLibrary.Repository;
using FinanceLibrary.Repository.Interfaces;
using FinanceLibrary.Repository.SqlContexts;

namespace FinanceLibrary.Logic
{
    public class DeleteLogic
    {
        private readonly IDeleteContext _context;
        private readonly Database _database;

        public DeleteLogic()
        {
            _database = new Database();
            _context = new DeleteSqlContext(_database);
        }

        /// <summary>
        /// Deletes a balance from the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the balance to delete.</param>
        /// <returns>Whether or not the action was a success.</returns>
        public bool DeleteBalance(User user, int id)
        {
            try
            {
                if (user.GetBalance(id) == null) return false;

                _context.DeleteBalance(id);

                user.DeleteBalance(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes a payment from the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the payment.</param>
        /// <returns>Whether or not the action was a success.</returns>
        public bool DeletePayment(User user, int id)
        {
            try
            {
                if (user.GetPayment(id) == null) return false;

                _context.DeletePayment(id);

                user.DeletePayment(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes transaction from the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the transaction.</param>
        /// <returns></returns>
        public bool DeleteTransaction(User user, int id)
        {
            try
            {
                if (user.GetTransaction(id) == null) return false;

                if (!(user.GetPaymentByTransaction(id) is Payment payment)) return false;

                _context.DeleteTransaction(id);

                payment.DeleteTransaction(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }
    }
}