using System;
using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DataSqlContext : IDataContext
    {
        public User CreateUser(string name, string lastName, string email, string password)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "INSERT INTO \"USER\" (NAME, LASTNAME, EMAIL, PASSWORD) VALUES (@name, @lastName, @email, @password)",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@name", name));
            command.Parameters.Add(new MySqlParameter("@lastName", lastName));
            command.Parameters.Add(new MySqlParameter("@email", email));
            command.Parameters.Add(new MySqlParameter("@password", password));

            command.ExecuteNonQuery();

            return LoginUser(email, password, false, false, false);
        }

        public User LoginUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, NAME, LASTNAME FROM USER WHERE EMAIL = @email AND PASSWORD = @password",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@email", email));
            command.Parameters.Add(new MySqlParameter("@password", password));

            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read()) throw new WrongUsernameOrPasswordException();

            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.GetString(2);

            User user = new User(id, name, lastName, email);

            reader.Close();

            if (loadBankAccounts) user.AddBankAccounts(GetBankAccountsOfUser(id)); 

            if (loadPayments) user.AddPayment(GetPaymentsOfUser(id));

            return user;
        }

        public List<Balance> GetBankAccountsOfUser(int userId)
        {
            var bankAccounts =  new List<Balance>();

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, BALANCE, NAME FROM BANKACCOUNT WHERE USER_ID = @userId", conneciton)
                { CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal balance = Convert.ToDecimal(reader.GetString(1));
                string name = reader.GetString(2);

                bankAccounts.Add(new Balance(id, name, balance));
            }

            reader.Close();

            return bankAccounts;
        }

        public List<IPayment> GetPaymentsOfUser(int userId)
        {
            var payments = new List<IPayment>();
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, NAME, AMOUNT, TYPE FROM PAYMENT WHERE USER_ID = @userId", connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal amount = Convert.ToDecimal(reader.GetString(2));
                PaymentType type = (PaymentType) Enum.Parse(typeof(PaymentType) ,reader.GetString(3));

                switch (type)
                {
                    case PaymentType.MonthlyBill:
                        payments.Add(new MonthlyBill(id, name, amount));
                        break;
                    case PaymentType.MonthlyIncome:
                        payments.Add(new MonthlyIncome(id, name, amount));
                        break;
                    case PaymentType.DailyBill:
                        payments.Add(new MonthlyBill(id, name, amount));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            reader.Close();

            payments.ForEach(p => p.AddTransactions(GetTransactionsOfPayment(p.Id)));

            return payments;
        }

        public List<Transaction> GetTransactionsOfPayment(int paymentId)
        {
            var transactions = new List<Transaction>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, AMOUNT, DESCRIPTION FROM TRANSACTION WHERE PAYMENT_ID = @paymentId", connecion)
                {CommandType =  CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@paymentId", paymentId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal amount = Convert.ToDecimal(reader.GetString(1));
                string description = reader.GetString(2);

                transactions.Add(new Transaction(id, amount, description));
            }

            reader.Close();

            return transactions;
        }
    }
}