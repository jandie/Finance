using System;
using System.Data;
using System.Globalization;
using Database.Interfaces;
using Library.Classes;
using Library.Exceptions;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class InsertSqlContext : IInsertContext, IDatabaseClosable
    {
        private readonly Database _db;

        public InsertSqlContext()
        {
            _db = new Database();
        }

        public void CloseDb()
        {
            _db.Close();
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("INSERT INTO BANKACCOUNT (USER_ID, NAME, BALANCE, NAMESALT, BALANCESALT) " +
                                     "VALUES (@userId, @name, @balance, @nameSalt, @balanceSalt)",
                        connection)
                    { CommandType = CommandType.Text };

                balance.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(balance.Name));
                balance.BalanceAmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(balance.BalanceAmount)));

                command.Parameters.Add(new MySqlParameter("@userId", userId));
                command.Parameters.Add(new MySqlParameter("@name", 
                    Encryption.Instance.EncryptText(balance.Name, password, balance.NameSalt)));
                command.Parameters.Add(new MySqlParameter("@balance", 
                    Encryption.Instance.EncryptText(balance.BalanceAmount.ToString(CultureInfo.InvariantCulture), password, balance.BalanceAmountSalt)));
                command.Parameters.Add(new MySqlParameter("@nameSalt", balance.NameSalt));
                command.Parameters.Add(new MySqlParameter("@balanceSalt", balance.BalanceAmountSalt));

                command.ExecuteNonQuery();

                return GetLastBankAccountId(userId);
            }
            catch (Exception)
            {
                
                throw new AddBankAccountException("Bank account could not be added.");
            }
            
        }

        /// <summary>
        /// Gets last bank account id of a user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns></returns>
        private int GetLastBankAccountId(int userId)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("SELECT MAX(Id) FROM BANKACCOUNT WHERE USER_ID = @userId",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@userId", userId));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception)
            {

                throw new AddBankAccountException("Bank account could not be added.");
            }

            throw new AddBankAccountException("Bank account could not be added.");
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand(
                        "INSERT INTO PAYMENT (USER_ID, NAME, AMOUNT, TYPE, NAMESALT, AMOUNTSALT) " +
                        "VALUES(@userId, @name, @amount, @type, @nameSalt, @amountSalt)",
                        connection)
                    { CommandType = CommandType.Text };

                payment.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(payment.Name));
                payment.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(payment.Amount)));

                command.Parameters.Add(new MySqlParameter("@userId", userId));
                command.Parameters.Add(new MySqlParameter("@name",
                    Encryption.Instance.EncryptText(payment.Name, password, payment.NameSalt)));
                command.Parameters.Add(new MySqlParameter("@amount", 
                    Encryption.Instance.EncryptText(payment.Amount.ToString(CultureInfo.InvariantCulture), password, payment.AmountSalt)));
                command.Parameters.Add(new MySqlParameter("@type", payment.PaymentType.ToString()));
                command.Parameters.Add(new MySqlParameter("@nameSalt", payment.NameSalt));
                command.Parameters.Add(new MySqlParameter("@amountSalt", payment.AmountSalt));

                command.ExecuteNonQuery();

                return GetLastPaymentId(userId);
            }
            catch (Exception)
            {
                
                throw new AddPaymentException("Payment couldn't be added.");
            }
        }

        /// <summary>
        /// Gets last payment id of a user.
        /// </summary>
        /// <param name="userId">The id of the user.</param>
        /// <returns></returns>
        private int GetLastPaymentId(int userId)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("SELECT MAX(Id) FROM PAYMENT WHERE USER_ID = @userId",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@userId", userId));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception)
            {

                throw new AddPaymentException("Payment couldn't be added.");
            }

            throw new AddPaymentException("Payment couldn't be added.");
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand(
                        "INSERT INTO TRANSACTION (PAYMENT_ID, AMOUNT, DESCRIPTION, DateAdded, AMOUNTSALT, DESCRIPTIONSALT) " +
                        "VALUES(@paymentId, @amount, @description, @dateAdded, @amountSalt, @descriptionSalt)",
                        connection)
                    { CommandType = CommandType.Text };

                transaction.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(transaction.Amount)));
                transaction.DescriptionSalt = Hashing.ExtractSalt(Hashing.CreateHash(transaction.Description));

                command.Parameters.Add(new MySqlParameter("@paymentId", paymentId));
                command.Parameters.Add(new MySqlParameter("@amount", 
                    Encryption.Instance.EncryptText(transaction.Amount.ToString(CultureInfo.InvariantCulture), password, transaction.AmountSalt)));
                command.Parameters.Add(new MySqlParameter("@description", 
                    Encryption.Instance.EncryptText(transaction.Description, password, transaction.DescriptionSalt)));
                command.Parameters.Add(new MySqlParameter("@dateAdded", DateTime.Now.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new MySqlParameter("@amountSalt", transaction.AmountSalt));
                command.Parameters.Add(new MySqlParameter("@descriptionSalt", transaction.DescriptionSalt));

                command.ExecuteNonQuery();

                return GetLastTransactiontId(paymentId);
            }
            catch (Exception)
            {
                
                throw new AddTransactionException("Transaction couldn't be added.");
            }
        }

        /// <summary>
        /// Gets the last transaction id of a payment.
        /// </summary>
        /// <param name="paymentId">The id of the payment.</param>
        /// <returns></returns>
        private int GetLastTransactiontId(int paymentId)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("SELECT MAX(Id) FROM TRANSACTION WHERE PAYMENT_ID = @paymentId",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@paymentId", paymentId));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception)
            {

                throw new AddTransactionException("Transaction couldn't be added.");
            }

            throw new AddTransactionException("Transaction couldn't be added.");
        }
    }
}