using System.Data;
using Database.Interfaces;
using Library.Enums;
using Oracle.ManagedDataAccess.Client;

namespace Database
{
    public class InsertSqlContext : IInsertContext
    {
        public void AddBankAccount(int userId, string name, decimal balance)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("INSERT INTO BANKACCOUNT (USER_ID, NAME, BALANCE) VALUES (:userId, :name, :balance)", connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new OracleParameter(":userId", userId));
            command.Parameters.Add(new OracleParameter(":name", name));
            command.Parameters.Add(new OracleParameter(":balance", balance.ToString()));

            command.ExecuteNonQuery();
        }

        public void AddPayment(int userId, string name, decimal amount, PaymentType type)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("INSERT INTO PAYMENT (USER_ID, NAME, AMOUNT, TYPE) VALUES(:userId, :name, :amount, :type)", connection)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new OracleParameter(":userId", userId));
            command.Parameters.Add(new OracleParameter(":name", name));
            command.Parameters.Add(new OracleParameter(":amount", amount.ToString()));
            command.Parameters.Add(new OracleParameter(":type", type.ToString()));

            command.ExecuteNonQuery();
        }

        public void AddTransaction(int paymentId, decimal amount, string description)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("INSERT INTO TRANSACTION (PAYMENT_ID, AMOUNT, DESCRIPTION) VALUES(:paymentId, :amount, :description)", connection)
            { CommandType = CommandType.Text };

            command.Parameters.Add(new OracleParameter(":paymentId", paymentId));
            command.Parameters.Add(new OracleParameter(":amount", amount.ToString()));
            command.Parameters.Add(new OracleParameter(":description", description));

            command.ExecuteNonQuery();
        }
    }
}
