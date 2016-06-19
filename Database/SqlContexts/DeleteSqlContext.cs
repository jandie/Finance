using System.Data;
using Database.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace Database.SqlContexts
{
    public class DeleteSqlContext : IDeleteContext
    {
        public void DeleteBalance(int id)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("DELETE FROM BANKACCOUNT WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));

            command.ExecuteNonQuery();
        }

        public void DeletePayment(int id)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("DELETE FROM PAYMENT WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));

            command.ExecuteNonQuery();
        }

        public void DeleteTransaction(int id)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("DELETE FROM TRANSACTION WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));

            command.ExecuteNonQuery();
        }
    }
}
