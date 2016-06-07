using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes;
using Oracle.ManagedDataAccess.Client;

namespace Database
{
    public class DataSqlContext : IDataContext
    {
        public User CreateUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, NAME, LASTNAME FROM \"USER\" WHERE PASSWORD = :password AND EMAIL = :email", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":email", email));
            command.Parameters.Add(new OracleParameter(":password", password));

            OracleDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastName = reader.GetString(2);

                User user = new User(id, name, lastName, password);

                return user;
            }

            return null;
        }

        public User LoginUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, NAME, LASTNAME FROM \"USER\" WHERE PASSWORD = :password AND EMAIL = :email", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":email", email));
            command.Parameters.Add(new OracleParameter(":password", password));

            OracleDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastName = reader.GetString(2);

                User user = new User(id, name, lastName, password);

                return user;
            }

            return null;
        }
        
    }
}