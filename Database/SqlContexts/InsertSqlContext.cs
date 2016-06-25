using System.Data;
using Database.Interfaces;
using Library.Enums;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class InsertSqlContext : IInsertContext
    {
        public void AddBankAccount(int userId, string name, decimal balance)
        {
            MySqlConnection connection = Database.Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("INSERT INTO BANKACCOUNT (USER_ID, NAME, BALANCE) VALUES (:userId, :name, :balance)", connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter(":userId", userId));
            command.Parameters.Add(new MySqlParameter(":name", name));
            command.Parameters.Add(new MySqlParameter(":balance", balance.ToString()));

            command.ExecuteNonQuery();
        }

        public void AddPayment(int userId, string name, decimal amount, PaymentType type)
        {
            MySqlConnection connection = Database.Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("INSERT INTO PAYMENT (USER_ID, NAME, AMOUNT, TYPE) VALUES(:userId, :name, :amount, :type)", connection)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter(":userId", userId));
            command.Parameters.Add(new MySqlParameter(":name", name));
            command.Parameters.Add(new MySqlParameter(":amount", amount.ToString()));
            command.Parameters.Add(new MySqlParameter(":type", type.ToString()));

            command.ExecuteNonQuery();
        }

        public void AddTransaction(int paymentId, decimal amount, string description)
        {
            MySqlConnection connection = Database.Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("INSERT INTO TRANSACTION (PAYMENT_ID, AMOUNT, DESCRIPTION) VALUES(:paymentId, :amount, :description)", connection)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter(":paymentId", paymentId));
            command.Parameters.Add(new MySqlParameter(":amount", amount.ToString()));
            command.Parameters.Add(new MySqlParameter(":description", description));

            command.ExecuteNonQuery();
        }
    }
}
