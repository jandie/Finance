using System.Data;
using System.Globalization;
using Database.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace Database.SqlContexts
{
    public class ChangeSqlContext : IChangeContext
    {
        public void ChangeBalance(int id, string name, decimal balanceAmount)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("UPDATE BANKACCOUNT SET NAME = :name, BALANCE = :balanceAmount WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));
            command.Parameters.Add(new OracleParameter(":name", name));
            command.Parameters.Add(new OracleParameter(":balanceAmount", balanceAmount.ToString(CultureInfo.InvariantCulture)));

            command.ExecuteNonQuery();
        }

        public void ChangePayment(int id, string name, decimal amount)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("UPDATE PAYMENT SET NAME = :name, AMOUNT = :amount WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));
            command.Parameters.Add(new OracleParameter(":name", name));
            command.Parameters.Add(new OracleParameter(":amount", amount.ToString(CultureInfo.InvariantCulture)));

            command.ExecuteNonQuery();
        }

        public void ChangeTransaction(int id, decimal amount, string description)
        {
            OracleConnection connection = Database.Database.Instance.Connection;
            OracleCommand command = new OracleCommand("UPDATE TRANSACTION SET AMOUNT = :amount, DESCRIPTION = :description WHERE ID = :id", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":id", id));
            command.Parameters.Add(new OracleParameter(":amount", amount.ToString(CultureInfo.InvariantCulture)));
            command.Parameters.Add(new OracleParameter(":description", description));

            command.ExecuteNonQuery();
        }
    }
}
