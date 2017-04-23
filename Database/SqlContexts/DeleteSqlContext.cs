using System;
using System.Data;
using System.Diagnostics;
using Database.Interfaces;
using Library.Exceptions;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DeleteSqlContext : IDeleteContext, IDatabaseClosable
    {
        private readonly Database _db;

        public DeleteSqlContext()
        {
            _db = Database.NewInstance;
        }

        public void CloseDb()
        {
            _db.Close();
        }

        /// <summary>
        /// Deactivates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        public void DeactivateUser(int id)
        {
            ChangeUserActiveStatus(id, 0);
        }

        /// <summary>
        /// Activates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        public void ActivateUser(int id)
        {
            ChangeUserActiveStatus(id, 1);
        }

        /// <summary>
        /// Changes the status of the user to active (1) or not inactive(0).
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="status">The new status of the user.</param>
        private void ChangeUserActiveStatus(int id, int status)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command = new MySqlCommand("UPDATE USER SET ACTIVE = @status WHERE ID = @id", connection)
                {
                    CommandType = CommandType.Text
                };

                command.Parameters.Add(new MySqlParameter("@status", status));
                command.Parameters.Add(new MySqlParameter("@id", id));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeleteBalanceException("User could not be (de)activated.");
            }
        }

        /// <summary>
        /// Deactivates a balance in the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        public void DeleteBalance(int id)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
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
                MySqlConnection connection = _db.Connection;
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
                MySqlConnection connection = _db.Connection;
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