using System;
using System.Collections.Generic;
using System.Globalization;
using Database.Interfaces;
using Library.Classes;
using Library.Exceptions;

namespace Database.SqlContexts
{
    public class InsertSqlContext : IInsertContext
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public InsertSqlContext(Database database)
        {
            _db = database;
            _encryption = new Encryption();
        }

        /// <summary>
        /// Adds a balance to the databse.
        /// </summary>
        /// <param name="userId">The id of the user the balance belongs to.</param>
        /// <param name="balance">The new balance.</param>
        /// <param name="password">The password used of decrypting data.</param>
        public int AddBankAccount(int userId, Balance balance, string password)
        {
            try
            {
                const string query = "INSERT INTO BANKACCOUNT (USER_ID, NAME, BALANCE, NAMESALT, BALANCESALT) " +
                                     "VALUES (@userId, @name, @balance, @nameSalt, @balanceSalt)";

                balance.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(balance.Name));
                balance.BalanceAmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(balance.BalanceAmount, CultureInfo.InvariantCulture)));

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"userId", userId},
                    {"name", _encryption.EncryptText(balance.Name, password, balance.NameSalt)},
                    {"balance", _encryption.EncryptText(balance.BalanceAmount.ToString(CultureInfo.InvariantCulture), password, balance.BalanceAmountSalt)},
                    {"nameSalt", balance.NameSalt},
                    {"balanceSalt", balance.BalanceAmountSalt}
                };

                return Convert.ToInt32(_db.Execute(query, parameters, Database.QueryType.Insert));
            }
            catch (Exception)
            {
                throw new AddBankAccountException("Bank account could not be added.");
            }
        }

        /// <summary>
        /// Adds a payment to the database.
        /// </summary>
        /// <param name="userId">The id of the user the payment belongs to.</param>
        /// <param name="payment">The new payment.</param>
        /// <param name="password">The password used of decrypting data.</param>
        public int AddPayment(int userId, Payment payment, string password)
        {
            try
            {
                const string query = "INSERT INTO PAYMENT (USER_ID, NAME, AMOUNT, TYPE, NAMESALT, AMOUNTSALT) " +
                                     "VALUES(@userId, @name, @amount, @type, @nameSalt, @amountSalt)";

                payment.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(payment.Name));
                payment.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(payment.Amount, CultureInfo.InvariantCulture)));

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"userId", userId},
                    {"name", _encryption.EncryptText(payment.Name, password, payment.NameSalt)},
                    {"amount", _encryption.EncryptText(payment.Amount.ToString(CultureInfo.InvariantCulture), password, payment.AmountSalt)},
                    {"type", payment.PaymentType.ToString()},
                    {"nameSalt", payment.NameSalt},
                    {"amountSalt", payment.AmountSalt}
                };

                return Convert.ToInt32(_db.Execute(query, parameters, Database.QueryType.Insert));
            }
            catch (Exception)
            {
                throw new AddPaymentException("Payment couldn't be added.");
            }
        }

        /// <summary>
        /// Adds a transaction to the databse.
        /// </summary>
        /// <param name="paymentId">The id of the payment the transaction belongs to.</param>
        /// <param name="transaction">The new transaction.</param>
        /// <param name="password">The password used of decrypting data.</param>
        public int AddTransaction(int paymentId, Transaction transaction, string password)
        {
            try
            {
                const string query =
                    "INSERT INTO TRANSACTION (PAYMENT_ID, AMOUNT, DESCRIPTION, DateAdded, AMOUNTSALT, DESCRIPTIONSALT) " +
                    "VALUES(@paymentId, @amount, @description, @dateAdded, @amountSalt, @descriptionSalt)";

                transaction.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(transaction.Amount, CultureInfo.InvariantCulture)));
                transaction.DescriptionSalt = Hashing.ExtractSalt(Hashing.CreateHash(transaction.Description));

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"paymentId", paymentId},
                    {"amount", _encryption.EncryptText(transaction.Amount.ToString(CultureInfo.InvariantCulture), password, transaction.AmountSalt)},
                    {"description", _encryption.EncryptText(transaction.Description, password, transaction.DescriptionSalt)},
                    {"dateAdded", DateTime.Now.ToString("yyyy-MM-dd")},
                    {"amountSalt", transaction.AmountSalt},
                    {"descriptionSalt", transaction.DescriptionSalt}
                };

                return Convert.ToInt32(_db.Execute(query, parameters, Database.QueryType.Insert));
            }
            catch (Exception)
            {
                throw new AddTransactionException("Transaction couldn't be added.");
            }
        }
    }
}