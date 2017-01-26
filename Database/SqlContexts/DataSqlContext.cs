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
        private readonly Database _db;

        public DataSqlContext()
        {
            _db = Database.Instance;
        }

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
            MySqlConnection connection = _db.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD, CURRENCY, LANGUAGE) VALUES (@name, @lastName, @email, @password, @currencyId, @languageId)",
                    connection) {CommandType = CommandType.Text};
            string hash = Hashing.CreateHash(password);
            string salt = Hashing.ExtractSalt(hash);
            command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(name, password, salt)));
            command.Parameters.Add(new MySqlParameter("@lastName", Encryption.Instance.EncryptText(lastName, password, salt)));
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
            User user;

            MySqlConnection connection = _db.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML, U.PASSWORD, U.NAMESALT, U.LASTNAMESALT FROM USER U " +
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

                string hash = reader.GetString(8);
                string salt = Hashing.ExtractSalt(hash);
                int id = reader.GetInt32(0);
                
                int languageId = reader.GetInt32(3);
                int currencyId = reader.GetInt32(4);
                string currencyAbbrevation = reader.GetString(5);
                string currencyName = reader.GetString(6);
                string currencyHtml = reader.GetString(7);

                string name;
                string lastName;

                Currency currency = new Currency(currencyId, currencyAbbrevation, currencyName, currencyHtml);

                if (!Hashing.ValidatePassword(password, hash)) throw new WrongUsernameOrPasswordException();

                //Ensure backwards compability with old encryption protocol
                if (reader.IsDBNull(9) || reader.IsDBNull(10))
                {
                    name = Encryption.Instance.DecryptText(reader.GetString(1), password, salt);
                    lastName = Encryption.Instance.DecryptText(reader.GetString(2), password, salt);

                    reader.Close();

                    user = new User(id, name, lastName, email, languageId, currency, UpdateToken(email), salt);

                    new ChangeSqlContext().ChangeUser(user, password);
                }
                else
                {
                    name = Encryption.Instance.DecryptText(reader.GetString(1), password, reader.GetString(9));
                    lastName = Encryption.Instance.DecryptText(reader.GetString(2), password, reader.GetString(10));

                    reader.Close();

                    user = new User(id, name, lastName, email, languageId, currency, UpdateToken(email), salt);
                }

                GetBalancesOfUser(id, password, salt).ForEach(b => user.AddBalance(b));
                GetPaymentsOfUser(id, password, salt).ForEach(p => user.AddPayment(p));
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

            MySqlConnection connection = _db.Connection;
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

            MySqlConnection connection = _db.Connection;
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
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <returns>List of balances of the user.</returns>
        public List<Balance> GetBalancesOfUser(int userId, string password, string salt)
        {
            List<Balance> bankAccounts = new List<Balance>();

            MySqlConnection conneciton = _db.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, BALANCE, NAME, BALANCESALT, NAMESALT " +
                                 "FROM BANKACCOUNT " +
                                 "WHERE USER_ID = @userId AND Active = 1",
                    conneciton)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    decimal balance;
                    string name;

                    //Ensure backwards compability with old encryption protocol
                    if (reader.IsDBNull(3) || reader.IsDBNull(4)) 
                    {
                        balance = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(1), password, salt));
                        name = Encryption.Instance.DecryptText(reader.GetString(2), password, salt);

                        Balance objBalance = new Balance(id, name, balance);

                        new ChangeSqlContext().ChangeBalance(objBalance, password);
                    }
                    else
                    {
                        string balanceSalt = reader.GetString(3);
                        string nameSalt = reader.GetString(4);

                        balance = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(1), password, balanceSalt));
                        name = Encryption.Instance.DecryptText(reader.GetString(2), password, nameSalt);
                    }

                    bankAccounts.Add(new Balance(id, name, balance));
                }
            }

            return bankAccounts;
        }

        /// <summary>
        /// Loads payments of a user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <returns>List of payments of the user.</returns>
        public List<IPayment> GetPaymentsOfUser(int userId, string password, string salt)
        {
            List<IPayment> payments = new List<IPayment>();

            MySqlConnection connection = _db.Connection;
            MySqlCommand command =
                new MySqlCommand("SELECT ID, NAME, AMOUNT, TYPE, NAMESALT, AMOUNTSALT " +
                                 "FROM PAYMENT " +
                                 "WHERE USER_ID = @userId AND Active = 1",
                    connection)
                {CommandType = CommandType.Text};

            command.Parameters.Add(new MySqlParameter("@userId", userId));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name;
                    decimal amount;
                    PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), reader.GetString(3));

                    //Ensure backwards compability with old encryption protocol
                    if (reader.IsDBNull(4) || reader.IsDBNull(5))
                    {
                        name = Encryption.Instance.DecryptText(reader.GetString(1), password, salt);
                        amount = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(2), password, salt));

                        Payment payment = new MonthlyBill(id, name, amount, type);
                        
                        new ChangeSqlContext().ChangePayment(payment, password);
                    }
                    else
                    {
                        string nameSalt = reader.GetString(4);
                        string amountSalt = reader.GetString(5);

                        name = Encryption.Instance.DecryptText(reader.GetString(1), password, nameSalt);
                        amount = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(2), password, amountSalt));
                    }

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

            payments.ForEach(p => GetTransactionsOfPayment(p, password, salt, 
                DateTime.Now.ToString("yyyy-MM")).ForEach(p.AddTransaction));

            return payments;
        }

        /// <summary>
        /// Loads the transactions of a payment from the database.
        /// </summary>
        /// <param name="payment">The payment itself.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        /// <param name="monthYear">Year and month of the transactions to load. 
        /// Leave empty to load all transactions.
        /// Example: 2015-01</param>
        /// <returns>List of transactions of the payment.</returns>
        public List<Transaction> GetTransactionsOfPayment(IPayment payment, string password, string salt, string monthYear = null)
        {
            List<Transaction> transactions = new List<Transaction>();

            MySqlConnection connecion = _db.Connection;
            MySqlCommand command =
                new MySqlCommand(
                    "SELECT ID, AMOUNT, DESCRIPTION, AMOUNTSALT, DESCRIPTIONSALT " +
                    "FROM TRANSACTION " +
                    "WHERE PAYMENT_ID = @paymentId AND DateAdded LIKE @month AND Active = 1",
                    connecion)
                {CommandType = CommandType.Text};

            if (string.IsNullOrWhiteSpace(monthYear)) monthYear = "";

            command.Parameters.Add(new MySqlParameter("@paymentId", payment.Id));
            command.Parameters.Add(new MySqlParameter("@month", $"%{monthYear}%"));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    decimal amount;
                    string description;

                    //Ensure backwards compability with old encryption protocol
                    if (reader.IsDBNull(3) || reader.IsDBNull(4))
                    {
                        amount = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(1), password, salt));
                        description = Encryption.Instance.DecryptText(reader.GetString(2), password, salt);

                        Transaction transaction = new Transaction(id, amount, description, false);

                        new ChangeSqlContext().ChangeTransaction(transaction, password);
                    }
                    else
                    {
                        string amountSalt = reader.GetString(3);
                        string descriptionSalt = reader.GetString(4);

                        amount = Convert.ToDecimal(Encryption.Instance.DecryptText(reader.GetString(1), password, amountSalt));
                        description = Encryption.Instance.DecryptText(reader.GetString(2), password, descriptionSalt);
                    }

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

            MySqlConnection connecion = _db.Connection;
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

            MySqlConnection connecion = _db.Connection;
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