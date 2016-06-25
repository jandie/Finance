using System.Data;
using System.Globalization;
using Database.Interfaces;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class ChangeSqlContext : IChangeContext
    {
        public void ChangeBalance(int id, string name, decimal balanceAmount)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("UPDATE BANKACCOUNT SET NAME = @name, BALANCE = @balanceAmount WHERE ID = @id",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@name", name));
            command.Parameters.Add(new MySqlParameter("@balanceAmount", balanceAmount));
            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }

        public void ChangePayment(int id, string name, decimal amount)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("UPDATE PAYMENT SET NAME = @name, AMOUNT = @amount WHERE ID = @id",
                connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@name", name));
            command.Parameters.Add(new MySqlParameter("@amount", amount.ToString(CultureInfo.InvariantCulture)));
            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }

        public void ChangeTransaction(int id, decimal amount, string description)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("UPDATE TRANSACTION SET AMOUNT = @amount, DESCRIPTION = @description WHERE ID = @id",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@amount", amount.ToString(CultureInfo.InvariantCulture)));
            command.Parameters.Add(new MySqlParameter("@description", description));
            command.Parameters.Add(new MySqlParameter("@id", id));

            command.ExecuteNonQuery();
        }
    }
}