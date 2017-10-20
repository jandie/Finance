using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Database.Interfaces;
using Library.Classes;
using Library.Exceptions;

namespace Database.SqlContexts
{
    public class ChangeSqlContext : IChangeContext
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public ChangeSqlContext(Database database)
        {
            _db = database;
            _encryption = new Encryption();
        }

        /// <summary>
        /// Changes a balance in the database.
        /// </summary>
        /// <param name="balance">The balance to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        public void ChangeBalance(Balance balance, string password)
        {
            try
            {
                const string query = "UPDATE BANKACCOUNT " +
                                     "SET NAME = @name, BALANCE = @balanceAmount, NAMESALT = @nameSalt, BALANCESALT = @balanceSalt " +
                                     "WHERE ID = @id";

                balance.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(balance.Name));
                balance.BalanceAmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(balance.BalanceAmount, CultureInfo.InvariantCulture)));

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"name",  _encryption.EncryptText(balance.Name, password, balance.NameSalt)},
                    {"balanceAmount",  _encryption.EncryptText(Convert.ToString(balance.BalanceAmount, CultureInfo.InvariantCulture), password, balance.BalanceAmountSalt)},
                    {"nameSalt",  balance.NameSalt},
                    {"balanceSalt",  balance.BalanceAmountSalt},
                    {"id", balance.Id},
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new ChangeBalanceException("Balance could not be changed.");
            }
        }

        /// <summary>
        /// Changes a payment in the database.
        /// </summary>
        /// <param name="payment">The payment to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        public void ChangePayment(Payment payment, string password)
        {
            try
            {
                const string query = "UPDATE PAYMENT " +
                                     "SET NAME = @name, AMOUNT = @amount, NAMESALT = @nameSalt, AMOUNTSALT = @amountSalt " +
                                     "WHERE ID = @id";

                payment.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(payment.Name));
                payment.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(payment.Amount, CultureInfo.InvariantCulture)));

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"name", _encryption.EncryptText(payment.Name, password, payment.NameSalt)},
                    {"amount", _encryption.EncryptText(payment.Amount.ToString(CultureInfo.InvariantCulture), password, payment.AmountSalt)},
                    {"nameSalt", payment.NameSalt},
                    {"amountSalt", payment.AmountSalt},
                    {"id", payment.Id}
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new ChangePaymentException("Payment could not be changed.");
            }
        }

        /// <summary>
        /// Changes a transaction in the database.
        /// </summary>
        /// <param name="transaction">The transaction to be saved.</param>
        /// <param name="password">Password used for encryption.</param>
        public void ChangeTransaction(Transaction transaction, string password)
        {
            try
            {
                const string query = "UPDATE TRANSACTION " +
                                     "SET AMOUNT = @amount, DESCRIPTION = @description, AMOUNTSALT = @amountSalt, DESCRIPTIONSALT = @descriptionSalt " +
                                     "WHERE ID = @id";

                transaction.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(transaction.Amount, CultureInfo.InvariantCulture)));
                transaction.DescriptionSalt = Hashing.ExtractSalt(Hashing.CreateHash(transaction.Description));

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"amount", _encryption.EncryptText(transaction.Amount.ToString(CultureInfo.InvariantCulture), password, transaction.AmountSalt)},
                    {"description", _encryption.EncryptText(transaction.Description, password, transaction.DescriptionSalt)},
                    {"amountSalt", transaction.AmountSalt},
                    {"descriptionSalt", transaction.DescriptionSalt},
                    {"id", transaction.Id}
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new ChangeTransactionException("Transaction could not be changed.");
            }
        }

        /// <summary>
        /// Changes everything but the password of a user in the database.
        /// </summary>
        /// <param name="user">The changed user to save.</param>
        /// <param name="password">Password used for encryption.</param>
        public void ChangeUser(User user, string password)
        {
            try
            {
                const string query = "UPDATE USER " +
                                     "SET NAME = @name, LASTNAME = @lastName, CURRENCY = @currencyId, LANGUAGE = @languageId, NAMESALT = @nameSalt, LASTNAMESALT = @lastNameSalt " +
                                     "WHERE EMAIL = @email";

                string nameSalt = Hashing.GenerateSalt();
                string lastNameSalt = Hashing.GenerateSalt();

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"name", _encryption.EncryptText(user.Name, password, nameSalt)},
                    {"lastName", _encryption.EncryptText(user.LastName, password, lastNameSalt)},
                    {"currencyId", user.Currency.Id},
                    {"languageId", user.LanguageId},
                    {"email", user.Email},
                    {"nameSalt", nameSalt},
                    {"lastNameSalt", lastNameSalt}
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                
                throw new ChangeUserException("User could not be changed.");
            }
        }

        /// <summary>
        /// Changes a password of a user in the database.
        /// </summary>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="newPassword">The new password of the user.</param>
        public void ChangePassword(string email, string newPassword)
        {
            try
            {
                const string query = "UPDATE USER SET PASSWORD = @newPassword " +
                                     "WHERE EMAIL = @email";

                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"newPassword", Hashing.CreateHash(newPassword)},
                    {"email", email}
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new ChangePasswordException("Password could not be changed.");
            }
        }
    }
}