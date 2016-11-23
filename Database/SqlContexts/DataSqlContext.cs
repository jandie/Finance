using System;
using System.Collections.Generic;
using System.Data;
using Database.Interfaces;
using Library.Classes;
using Library.Classes.Language;
using Library.Enums;
using Library.Exceptions;
using Library.Interfaces;
using Library.Utils;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class DataSqlContext : IDataContext
    {
        #region User

        /// <summary>
        /// Creates a new user and returns the user.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="currencyId">The id of the preferred currency.</param>
        /// <param name="languageId">The id of the preferred language.</param>
        /// <returns>A user that has been loaded from the database.</returns>
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

        /// <summary>
        /// Loads the user and checks whether or not the password is correct.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A user that has been loaded from the database.</returns>
        public User LoginUser(string email, string password)
        {
            User user = null;

            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML, U.PASSWORD, U.ENCRYPTED FROM USER U " +
                    "INNER JOIN CURRENCY C ON C.ID = U.CURRENCY WHERE EMAIL = @email AND ACTIVE = 1;",
                    connection) {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@email", email));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
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
                int encrypted = reader.GetInt32(9);

                reader.Close();

                if (!Hashing.ValidatePassword(password, hash)) throw new WrongUsernameOrPasswordException();

                if (encrypted > 0)
                {
                    Currency currency = new Currency(currencyId, currencyAbbrevation, currencyName, currencyHtml);
                    user = new User(id, name, lastName, email, languageId, currency, UpdateToken(email));

                    GetBalancesOfUser(id).ForEach(b => user.AddBalance(b));
                    GetPaymentsOfUser(id).ForEach(p => user.AddPayment(p));

                }
                else
                {
                    
                }
                
            }

            return user;
        }

        /// <summary>
        /// Updates the token in the databse and returns it.
        /// </summary>
        /// <param name="email">The email of the User where the token
        /// must be updated</param>
        /// <returns>The random token.</returns>
        private string UpdateToken(string email)
        {
            string ranString = RanUtil.RandomString(10);

            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "UPDATE user SET Token = @ranString WHERE email = @email",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@ranString", ranString));
            command.Parameters.Add(new MySqlParameter("@email", email));

            command.ExecuteNonQuery();

            return ranString;
        }

        /// <summary>
        /// Checks wether or not the token of the user was changed.
        /// </summary>
        /// <param name="email">The email of the user where the token is checked.</param>
        /// <param name="token">The token itself.</param>
        /// <returns>Wether or not the token is changed.</returns>
        public bool TokenChanged(string email, string token)
        {
            string tokenFromDb = null;

            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT TOKEN FROM USER WHERE EMAIL = @email",
                    connection)
                { CommandType = CommandType.Text };

            command.Parameters.Add(new MySqlParameter("@email", email));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tokenFromDb = reader.GetString(0);
                }
            }

            return tokenFromDb != token;
        }

        /// <summary>
        /// Loads all balances of the user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>List of balances of the user.</returns>
        private List<Balance> GetBalancesOfUser(int userId)
        {
            List<Balance> bankAccounts = new List<Balance>();

            MySqlConnection conneciton = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, BALANCE, NAME FROM BANKACCOUNT WHERE USER_ID = @userId AND Active = 1",
                    conneciton)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    decimal balance = reader.GetDecimal(1);
                    string name = reader.GetString(2);

                    bankAccounts.Add(new Balance(id, name, balance));
                }
            }

            return bankAccounts;
        }

        /// <summary>
        /// Loads payments of a user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns>List of payments of the user.</returns>
        private List<IPayment> GetPaymentsOfUser(int userId)
        {
            List<IPayment> payments = new List<IPayment>();
            MySqlConnection connection = Database.Instance.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, NAME, AMOUNT, TYPE FROM PAYMENT WHERE USER_ID = @userId AND Active = 1",
                    connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal amount = reader.GetDecimal(2);
                    PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), reader.GetString(3));

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
            }

            payments.ForEach(p => GetTransactionsOfPayment(p).ForEach(p.AddTransaction));

            return payments;
        }

        /// <summary>
        /// Loads the transactions of a payment from the database.
        /// </summary>
        /// <param name="payment">The payment itself.</param>
        /// <returns>List of transactions of the payment.</returns>
        private List<Transaction> GetTransactionsOfPayment(IPayment payment)
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

            using (MySqlDataReader reader = command.ExecuteReader())
            {
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
            }

            return transactions;
        }

        #endregion User

        /// <summary>
        /// Loads all exsisting currencies from the database.
        /// </summary>
        /// <returns>A list of all exsisting currencies.</returns>
        public List<Currency> LoadCurrencies()
        {
            List<Currency> currencies = new List<Currency>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, ABBREVATION, NAME, HTML FROM CURRENCY", connecion)
            { CommandType = CommandType.Text };

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string abbrevation = reader.GetString(1);
                    string name = reader.GetString(2);
                    string html = reader.GetString(3);

                    currencies.Add(new Currency(id, abbrevation, name, html));
                }
            }

            return currencies;
        }

        /// <summary>
        /// Loads all exsisting languages from the database.
        /// </summary>
        /// <returns>A list of all exsisting languages in the database.</returns>
        public List<Language> LoadLanguages()
        {
            List<Language> languages = new List<Language>();

            MySqlConnection connecion = Database.Instance.Connection;
            MySqlCommand command = new MySqlCommand("SELECT ID, ABBREVATION, NAME FROM LANGUAGE", connecion)
            { CommandType = CommandType.Text };

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string abbrevation = reader.GetString(1);
                    string name = reader.GetString(2);

                    languages.Add(new Language(id, abbrevation, name));
                }

            }

            return languages;
        }
    }
}