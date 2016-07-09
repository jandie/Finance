using System;
using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes;
using Library.Classes.Language;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DataSqlContext : IDataContext
    {
        public List<Currency> LoadCurrencies()
        {
            List<Currency> currencies = new List<Currency>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, ABBREVATION, NAME, HTML FROM CURRENCY", connecion)
            {CommandType = CommandType.Text};

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string abbrevation = reader.GetString(1);
                string name = reader.GetString(2);
                string html = reader.GetString(3);

                currencies.Add(new Currency(id, abbrevation, name, html));
            }

            reader.Close();

            return currencies;
        }

        public List<Language> LoadLanguages()
        {
            List<Language> languages = new List<Language>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, ABBREVATION, NAME FROM LANGUAGE", connecion)
            {CommandType = CommandType.Text};

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string abbrevation = reader.GetString(1);
                string name = reader.GetString(2);

                languages.Add(new Language(id, abbrevation, name));
            }

            reader.Close();

            return languages;
        }

        #region User

        public User CreateUser(string name, string lastName, string email, string password, int currencyId,
            int languageId)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD, CURRENCY, LANGUAGE) VALUES (@name, @lastName, @email, @password, @currencyId, @languageId)",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@name", name));
            command.Parameters.Add(new MySqlParameter("@lastName", lastName));
            command.Parameters.Add(new MySqlParameter("@email", email));
            command.Parameters.Add(new MySqlParameter("@password", Hashing.CreateHash(password)));
            command.Parameters.Add(new MySqlParameter("@currencyId", currencyId));
            command.Parameters.Add(new MySqlParameter("@languageId", languageId));

            command.ExecuteNonQuery();

            return LoginUser(email, password);
        }

        public User LoginUser(string email, string password)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML, U.PASSWORD FROM USER U " +
                    "INNER JOIN CURRENCY C ON C.ID = U.CURRENCY WHERE EMAIL = @email AND ACTIVE = 1;",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@email", email));

            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();

                throw new WrongUsernameOrPasswordException();
            }

            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.GetString(2);
            int languageId = reader.GetInt32(3);
            int currencyId = reader.GetInt32(4);
            string currencyAbbrevation = reader.GetString(5);
            string currencyName = reader.GetString(6);
            string currencyHtml = reader.GetString(7);
            string hash = reader.GetString(8);

            reader.Close();

            if (!Hashing.ValidatePassword(password, hash)) throw new WrongUsernameOrPasswordException();

            Currency currency = new Currency(currencyId, currencyAbbrevation, currencyName, currencyHtml);
            User user = new User(id, name, lastName, email, languageId, currency);

            user.AddBankAccounts(GetBankAccountsOfUser(id));
            user.AddPayments(GetPaymentsOfUser(id));

            return user;
        }

        public User LoadUser(string email)
        {
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML FROM USER U " +
                    "INNER JOIN CURRENCY C ON C.ID = U.CURRENCY WHERE EMAIL = @email AND ACTIVE = 1;",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@email", email));

            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();

                throw new WrongUsernameOrPasswordException();
            }

            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string lastName = reader.GetString(2);
            int languageId = reader.GetInt32(3);
            int currencyId = reader.GetInt32(4);
            string currencyAbbrevation = reader.GetString(5);
            string currencyName = reader.GetString(6);
            string currencyHtml = reader.GetString(7);

            reader.Close();

            Currency currency = new Currency(currencyId, currencyAbbrevation, currencyName, currencyHtml);
            User user = new User(id, name, lastName, email, languageId, currency);

            user.AddBankAccounts(GetBankAccountsOfUser(id));
            user.AddPayments(GetPaymentsOfUser(id));

            return user;
        }

        private List<Balance> GetBankAccountsOfUser(int userId)
        {
            List<Balance> bankAccounts = new List<Balance>();

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, BALANCE, NAME FROM BANKACCOUNT WHERE USER_ID = @userId AND Active = 1",
                    conneciton)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal balance = reader.GetDecimal(1);
                string name = reader.GetString(2);

                bankAccounts.Add(new Balance(id, name, balance));
            }

            reader.Close();

            return bankAccounts;
        }

        public List<IPayment> GetPaymentsOfUser(int userId)
        {
            List<IPayment> payments = new List<IPayment>();
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, NAME, AMOUNT, TYPE FROM PAYMENT WHERE USER_ID = @userId AND Active = 1",
                    connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal amount = reader.GetDecimal(2);
                PaymentType type = (PaymentType) Enum.Parse(typeof(PaymentType), reader.GetString(3));

                switch (type)
                {
                    case PaymentType.MonthlyBill:
                        payments.Add(new MonthlyBill(id, name, amount, type));
                        break;
                    case PaymentType.MonthlyIncome:
                        payments.Add(new MonthlyIncome(id, name, amount, type));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            reader.Close();

            payments.ForEach(p => p.AddTransactions(GetTransactionsOfPayment(p)));

            return payments;
        }

        public List<Transaction> GetTransactionsOfPayment(IPayment payment) //bug
        {
            List<Transaction> transactions = new List<Transaction>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT ID, AMOUNT, DESCRIPTION FROM TRANSACTION WHERE PAYMENT_ID = @paymentId AND DateAdded LIKE @month AND Active = 1",
                    connecion)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@paymentId", payment.Id));
            command.Parameters.Add(new MySqlParameter("@month", $"%-{DateTime.Now.ToString("MM")}-%"));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                decimal amount = reader.GetDecimal(1);
                string description = reader.GetString(2);

                if (payment is MonthlyBill)
                    transactions.Add(new Transaction(id, amount, description, false));
                else if (payment is MonthlyIncome)
                    transactions.Add(new Transaction(id, amount, description, true));
            }

            reader.Close();

            return transactions;
        }

        #endregion User
    }
}