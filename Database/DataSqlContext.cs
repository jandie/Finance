using System;
using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace Database
{
    public class DataSqlContext : IDataContext
    {
        public User CreateUser(string name, string lastName, string email, string password)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("INSERT INTO \"USER\" (NAME, LASTNAME, EMAIL, PASSWORD) VALUES (:name, :lastName, :email, :password)", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":name", name));
            command.Parameters.Add(new OracleParameter(":lastName", lastName));
            command.Parameters.Add(new OracleParameter(":email", email));
            command.Parameters.Add(new OracleParameter(":password", password));

            command.ExecuteNonQuery();

            return LoginUser(email, password, false, false, false);
        }

        public User LoginUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, NAME, LASTNAME FROM \"USER\" WHERE EMAIL = :email AND PASSWORD = :password", connection);
            command.CommandType = CommandType.Text;

            command.Parameters.Add(new OracleParameter(":email", email));
            command.Parameters.Add(new OracleParameter(":password", password));

            OracleDataReader reader = command.ExecuteReader();

            if (!reader.Read()) throw new WrongUsernameOrPasswordException();

            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.GetString(2);

            User user = new User(id, name, lastName, password);

            user.AddBankAccounts(GetBankAccountsOfUser(id)); 

            user.AddPayment(GetPaymentsOfUser(id));

            return user;
        }

        public List<BankAccount> GetBankAccountsOfUser(int userId)
        {
            List<BankAccount> bankAccounts =  new List<BankAccount>();

            OracleConnection conneciton = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, BALANCE, NAME FROM BANKACCOUNT WHERE USER_ID = :userId", conneciton)
                { CommandType = CommandType.Text};

            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal balance = Convert.ToDecimal(reader.GetString(1));
                string name = reader.GetString(2);

                bankAccounts.Add(new BankAccount(id, name, balance));
            }

            return bankAccounts;
        }

        public List<IPayment> GetPaymentsOfUser(int userId)
        {
            List<IPayment> payments = new List<IPayment>();
            OracleConnection connection = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, NAME, AMOUNT, TYPE FROM PAYMENT WHERE USER_ID = :userId", connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new OracleParameter(":userId", userId));

            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal amount = Convert.ToDecimal(reader.GetString(2));
                PaymentType type = (PaymentType) Enum.Parse(typeof(PaymentType) ,reader.GetString(3));

                switch (type)
                {
                    case PaymentType.ReocurringBill:
                        payments.Add(new ReocurringBill(id, name, amount));
                        break;
                    case PaymentType.ReocurringIncome:
                        payments.Add(new ReocurringIncome(id, name, amount));
                        break;
                    case PaymentType.IncrementalBill:
                        payments.Add(new IncrementalBill(id, name, amount));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Payment payment = payments[payments.Count - 1] as Payment;
                payment?.AddTransactions(GetTransactionsOfPayment(id));
            }

            return payments;
        }

        public List<Transaction> GetTransactionsOfPayment(int paymentId)
        {
            List<Transaction> transactions = new List<Transaction>();

            OracleConnection connecion = Database.Instance.Connection;
            OracleCommand command = new OracleCommand("SELECT ID, AMOUNT, DESCRIPTION FROM TRANSACTION WHERE PAYMENT_ID = :paymentId", connecion)
                {CommandType =  CommandType.Text};

            OracleDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal amount = Convert.ToDecimal(reader.GetString(1));
                string description = reader.GetString(2);

                transactions.Add(new Transaction(id, amount, description));
            }

            return transactions;
        }
    }
}