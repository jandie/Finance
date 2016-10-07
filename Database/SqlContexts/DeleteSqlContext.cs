using System;
using System.Data;
using System.Diagnostics;
using Database.Interfaces;
using Library.Exceptions;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DeleteSqlContext : IDeleteContext
    {
        /// <summary>
        /// Deactivates a balance in the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        public void DeleteBalance(int id)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command = new MySqlCommand("UPDATE BANKACCOUNT SET ACTIVE = 0 WHERE ID = @id", connection)
                {
                    CommandType = CommandType.Text
                };

                command.Parameters.Add(new MySqlParameter("@id", id));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                
                throw new DeleteBalanceException("Balance could not be deleted.");
            }
        }

        /// <summary>
        /// Deactivates a payment in the database.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        public void DeletePayment(int id)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command = new MySqlCommand("UPDATE PAYMENT SET ACTIVE = 0 WHERE ID = @id", connection)
                {
                    CommandType = CommandType.Text
                };

                command.Parameters.Add(new MySqlParameter("@id", id));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeletePaymentException("Payment could not be deleted.");
            }
        }

        /// <summary>
        /// Deactivates a transaction in the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        public void DeleteTransaction(int id)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command = new MySqlCommand("UPDATE TRANSACTION SET ACTIVE = 0 WHERE ID = @id", connection)
                {
                    CommandType = CommandType.Text
                };

                command.Parameters.Add(new MySqlParameter("@id", id));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeleteTransactionException("Transaction could not be deleted.");
            }
        }
    }
}