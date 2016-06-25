using System.Data;
using Database.Interfaces;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DeleteSqlContext : IDeleteContext
    {
        public void DeleteBalance(int id)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("DELETE FROM BANKACCOUNT WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new MySqlParameter(":id", id));

            command.ExecuteNonQuery();
        }

        public void DeletePayment(int id)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("DELETE FROM PAYMENT WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new MySqlParameter(":id", id));

            command.ExecuteNonQuery();
        }

        public void DeleteTransaction(int id)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("DELETE FROM TRANSACTION WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new MySqlParameter(":id", id));

            command.ExecuteNonQuery();
        }
    }
}
