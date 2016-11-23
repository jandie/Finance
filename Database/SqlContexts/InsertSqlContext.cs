using System;
using System.Data;
using System.Globalization;
using Database.Interfaces;
using Library.Enums;
using Library.Exceptions;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class InsertSqlContext : IInsertContext
    {
        /// <summary>
        /// Adds a balance to the databse.
        /// </summary>
        /// <param name="userId">The id of the user the balance belongs to.</param>
        /// <param name="name">The name of the balance.</param>
        /// <param name="balance">The balance of the balance.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        public int AddBankAccount(int userId, string name, decimal balance, string password, string salt)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command =
                    new MySqlCommand("INSERT INTO BANKACCOUNT (USER_ID, NAME, BALANCE) VALUES (@userId, @name, @balance)",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@userId", userId));
                command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(name, password, salt)));
                command.Parameters.Add(new MySqlParameter("@balance", Encryption.Instance.EncryptText(balance.ToString(CultureInfo.InvariantCulture), password, salt)));

                command.ExecuteNonQuery();

                return GetLastBankAccountId(userId);
            }
            catch (Exception)
            {
                
                throw new AddBankAccountException("Bank account could not be added.");
            }
            
        }

        private int GetLastBankAccountId(int userId)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
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
        /// <param name="name">The name of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <param name="type">The type of the payment.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        public int AddPayment(int userId, string name, decimal amount, PaymentType type, string password, string salt)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command =
                    new MySqlCommand(
                        "INSERT INTO PAYMENT (USER_ID, NAME, AMOUNT, TYPE) VALUES(@userId, @name, @amount, @type)",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@userId", userId));
                command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(name, password, salt)));
                command.Parameters.Add(new MySqlParameter("@amount", Encryption.Instance.EncryptText(amount.ToString(CultureInfo.InvariantCulture), password, salt)));
                command.Parameters.Add(new MySqlParameter("@type", type.ToString()));

                command.ExecuteNonQuery();

                return GetLastPaymentId(userId);
            }
            catch (Exception)
            {
                
                throw new AddPaymentException("Payment couldn't be added.");
            }
        }

        private int GetLastPaymentId(int userId)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
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
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <param name="password">The password used of decrypting data.</param>
        /// <param name="salt">The salt used for decrypting data.</param>
        public int AddTransaction(int paymentId, decimal amount, string description, string password, string salt)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
                MySqlCommand command =
                    new MySqlCommand(
                        "INSERT INTO TRANSACTION (PAYMENT_ID, AMOUNT, DESCRIPTION, DateAdded) VALUES(@paymentId, @amount, @description, @dateAdded)",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@paymentId", paymentId));
                command.Parameters.Add(new MySqlParameter("@amount", Encryption.Instance.EncryptText(amount.ToString(CultureInfo.InvariantCulture), password, salt)));
                command.Parameters.Add(new MySqlParameter("@description", Encryption.Instance.EncryptText(description, password, salt)));
                command.Parameters.Add(new MySqlParameter("@dateAdded", DateTime.Now.ToString("yyyy-MM-dd")));

                command.ExecuteNonQuery();

                return GetLastTransactiontId(paymentId);
            }
            catch (Exception)
            {
                
                throw new AddTransactionException("Transaction couldn't be added.");
            }
        }

        private int GetLastTransactiontId(int paymentId)
        {
            try
            {
                MySqlConnection connection = Database.Instance.Connection;
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