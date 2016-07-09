using System.Data;
using Database.Interfaces;
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
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("UPDATE BANKACCOUNT SET ACTIVE = 0 WHERE ID = @id", connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deactivates a payment in the database.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        public void DeletePayment(int id)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("UPDATE PAYMENT SET ACTIVE = 0 WHERE ID = @id", connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Deactivates a transaction in the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        public void DeleteTransaction(int id)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("UPDATE TRANSACTION SET ACTIVE = 0 WHERE ID = @id", connection)
            {
                CommandType = CommandType.Text
            };

            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }
    }
}