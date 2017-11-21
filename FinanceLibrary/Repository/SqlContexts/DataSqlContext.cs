using System;
using System.Collections.Generic;
using System.Data;
using FinanceLibrary.Classes;
using FinanceLibrary.Classes.Language;
using FinanceLibrary.Enums;
using FinanceLibrary.Exceptions;
using FinanceLibrary.Repository.Interfaces;
using FinanceLibrary.Utils;
using MlkPwgen;
using System.Globalization;

namespace FinanceLibrary.Repository.SqlContexts
{
    public class DataSqlContext : IDataContext
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public DataSqlContext(Database database)
        {
            _db = database;
            _encryption = new Encryption();
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
            const string query =
                "INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD, CURRENCY, LANGUAGE, NAMESALT, LASTNAMESALT, MASTERPASSWORD, MASTERSALT) " +
                "VALUES (@name, @lastName, @email, @password, @currencyId, @languageId, @nameSalt, @lastNameSalt, @masterPassword, @masterSalt)";

            string nameSalt = Hashing.GenerateSalt();
            string lastNameSalt = Hashing.GenerateSalt();
            string masterSalt = Hashing.GenerateSalt();
            string masterPassword = PasswordGenerator.Generate(20, Sets.Alphanumerics + Sets.FullSymbols);

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"name", _encryption.EncryptText(name, masterPassword, nameSalt)},
                {"lastName", _encryption.EncryptText(lastName, masterPassword, lastNameSalt)},
                {"email", email},
                {"password", Hashing.CreateHash(password)},
                {"currencyId", currencyId},
                {"languageId", languageId},
                {"nameSalt", nameSalt},
                {"lastNameSalt", lastNameSalt},
                {"masterPassword", _encryption.EncryptText(masterPassword, password, masterSalt)},
                {"masterSalt", masterSalt}
            };

            _db.Execute(query, parameters, Database.QueryType.NonQuery);

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
            const string query =
                "SELECT U.ID, U.NAME, U.LASTNAME, U.LANGUAGE, C.ID, C.Abbrevation, C.NAME, C.HTML, U.PASSWORD, U.NAMESALT, U.LASTNAMESALT, U.MASTERPASSWORD, U.MASTERSALT FROM USER U " +
                "INNER JOIN CURRENCY C ON C.ID = U.CURRENCY WHERE EMAIL = @email AND ACTIVE = 1;";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"email", email}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) throw new WrongUsernameOrPasswordException();
                DataRow row = table.Rows[0];

                string hash = row[8] as string;
                string salt = Hashing.ExtractSalt(hash);
                int id = Convert.ToInt32(row[0]);

                int languageId = Convert.ToInt32(row[3]);
                int currencyId = Convert.ToInt32(row[4]);
                string currencyAbbrevation = row[5] as string;
                string currencyName = row[6] as string;
                string currencyHtml = row[7] as string;

                Currency currency = new Currency(currencyId, currencyAbbrevation, currencyName, currencyHtml);

                if (!Hashing.ValidatePassword(password, hash)) throw new WrongUsernameOrPasswordException();
                if (string.IsNullOrWhiteSpace(row["MASTERPASSWORD"] as string)) FixOldEncryption(email, password);

                string masterPassword = _encryption.DecryptText(row["MASTERPASSWORD"] as string, password,
                    row["MASTERSALT"] as string);
                string name = _encryption.DecryptText(row[1] as string, masterPassword, row[9] as string);
                string lastName = _encryption.DecryptText(row[2] as string, masterPassword, row[10] as string);
                

                user = new User(id, name, lastName, email, languageId, currency, UpdateToken(email), salt)
                {
                    MasterPassword = masterPassword,
                    MasterSalt = row["MASTERSALT"] as string
                };

                GetBalancesOfUser(id, masterPassword).ForEach(b => user.AddBalance(b));
                GetPaymentsOfUser(id, masterPassword).ForEach(p => user.AddPayment(p));
            }

            return user;
        }

        /// <summary>
        /// When old encryption is used this method is used to change the masterpassword to the already exsisting password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        private void FixOldEncryption(string email, string password)
        {
            User user = new User(-1, "", "", email, -1, null, "", "")
            {
                MasterPassword = password
            };
            new ChangeSqlContext(_db).ChangePassword(user, password);
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
            const string query = "UPDATE user SET Token = @ranString WHERE email = @email";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"ranString",  ranString},
                {"email", email}
            };

            _db.Execute(query, parameters, Database.QueryType.NonQuery);

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
            const string query = "SELECT TOKEN FROM USER WHERE EMAIL = @email";

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "email", email } };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table != null && table.Rows.Count > 0)
                {
                    tokenFromDb = table.Rows[0][0] as string;
                }
            }

            return tokenFromDb != token;
        }

        /// <summary>
        /// Loads all balances of the user from the database.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <returns>List of balances of the user.</returns>
        public List<Balance> GetBalancesOfUser(int userId, string password)
        {
            List<Balance> bankAccounts = new List<Balance>();
            const string query = "SELECT ID, BALANCE, NAME, BALANCESALT, NAMESALT " +
                                 "FROM BANKACCOUNT " +
                                 "WHERE USER_ID = @userId AND Active = 1";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"userId", userId}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return bankAccounts;

                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string balanceSalt = row[3] as string;
                    string nameSalt = row[4] as string;
                    decimal balance = Convert.ToDecimal(_encryption.DecryptText(row[1] as string, password, balanceSalt), new CultureInfo("en-US"));
                    string name = _encryption.DecryptText(row[2] as string, password, nameSalt);

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
        /// <returns>List of payments of the user.</returns>
        public List<IPayment> GetPaymentsOfUser(int userId, string password)
        {
            List<IPayment> payments = new List<IPayment>();
            const string query = "SELECT ID, NAME, AMOUNT, TYPE, NAMESALT, AMOUNTSALT " +
                                 "FROM PAYMENT " +
                                 "WHERE USER_ID = @userId AND Active = 1";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"userId",  userId}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return payments;
                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), (string)row[3]);

                    string nameSalt = row[4] as string;
                    string amountSalt = row[5] as string;

                    string name = _encryption.DecryptText(row[1] as string, password, nameSalt);
                    decimal amount = Convert.ToDecimal(_encryption.DecryptText(row[2] as string, password, amountSalt), new CultureInfo("en-US"));

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

            payments.ForEach(p => GetTransactionsOfPayment(p, password,
                DateTime.Now.ToString("yyyy-MM")).ForEach(p.AddTransaction));

            return payments;
        }

        /// <summary>
        /// Loads the transactions of a payment from the database.
        /// </summary>
        /// <param name="payment">The payment itself.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="monthYear">Year and month of the transactions to load. 
        /// Leave empty to load all transactions.
        /// Example: 2015-01</param>
        /// <returns>List of transactions of the payment.</returns>
        public List<Transaction> GetTransactionsOfPayment(IPayment payment, string password, string monthYear = null)
        {
            List<Transaction> transactions = new List<Transaction>();
            const string query = "SELECT ID, AMOUNT, DESCRIPTION, AMOUNTSALT, DESCRIPTIONSALT " +
                                 "FROM TRANSACTION " +
                                 "WHERE PAYMENT_ID = @paymentId AND DateAdded LIKE @month AND Active = 1";

            if (string.IsNullOrWhiteSpace(monthYear)) monthYear = "";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"paymentId", payment.Id},
                {"month", $"%{monthYear}%"}
            };

            using (DataTable table = _db.Execute(query, parameters, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return transactions;
                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string amountSalt = row[3] as string;
                    string descriptionSalt = row[4] as string;
                    decimal amount = Convert.ToDecimal(_encryption.DecryptText(row[1] as string, password, amountSalt), new CultureInfo("en-US"));
                    string description = _encryption.DecryptText(row[2] as string, password, descriptionSalt);

                    switch (payment)
                    {
                        case MonthlyBill _:
                            transactions.Add(new Transaction(id, amount, description, false));
                            break;
                        case MonthlyIncome _:
                            transactions.Add(new Transaction(id, amount, description, true));
                            break;
                    }
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
            const string query = "SELECT ID, ABBREVATION, NAME, HTML FROM CURRENCY";

            using (DataTable table = _db.Execute(query, null, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return currencies;
                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string abbrevation = row[1] as string;
                    string name = row[2] as string;
                    string html = row[3] as string;

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
            const string query = "SELECT ID, ABBREVATION, NAME FROM LANGUAGE";

            using (DataTable table = _db.Execute(query, null, Database.QueryType.Return) as DataTable)
            {
                if (table == null || table.Rows.Count < 1) return languages;
                foreach (DataRow row in table.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string abbrevation = row[1] as string;
                    string name = row[2] as string;

                    languages.Add(new Language(id, abbrevation, name));
                }

            }

            return languages;
        }
    }
}