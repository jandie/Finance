﻿using System;
using System.Data;
using System.Diagnostics;
using Database.Interfaces;
using Library.Classes;
using Library.Exceptions;
using MySql.Data.MySqlClient;

namespace Database.SqlContexts
{
    public class ChangeSqlContext : IChangeContext
    {
        private Database _db;

        public ChangeSqlContext()
        {
            _db = Database.Instance;
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("UPDATE BANKACCOUNT " +
                                     "SET NAME = @name, BALANCE = @balanceAmount, NAMESALT = @nameSalt, BALANCESALT = @balanceSalt " +
                                     "WHERE ID = @id",
                        connection)
                    { CommandType = CommandType.Text };

                balance.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(balance.Name));
                balance.BalanceAmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(balance.BalanceAmount)));

                command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(balance.Name, password, balance.NameSalt)));
                command.Parameters.Add(new MySqlParameter("@balanceAmount", 
                    Encryption.Instance.EncryptText(Convert.ToString(balance.BalanceAmount), password, balance.BalanceAmountSalt)));
                command.Parameters.Add(new MySqlParameter("@nameSalt", balance.NameSalt));
                command.Parameters.Add(new MySqlParameter("@balanceSalt", balance.BalanceAmountSalt));
                command.Parameters.Add(new MySqlParameter("@id", balance.Id));

                command.ExecuteNonQuery();
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command = new MySqlCommand("UPDATE PAYMENT " +
                                                        "SET NAME = @name, AMOUNT = @amount, NAMESALT = @nameSalt, AMOUNTSALT = @amountSalt " +
                                                        "WHERE ID = @id",
                    connection)
                { CommandType = CommandType.Text };

                payment.NameSalt = Hashing.ExtractSalt(Hashing.CreateHash(payment.Name));
                payment.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(payment.Amount)));

                command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(payment.Name, password, payment.NameSalt)));
                command.Parameters.Add(new MySqlParameter("@amount", Encryption.Instance.EncryptText(payment.Amount.ToString(), password, payment.AmountSalt)));
                command.Parameters.Add(new MySqlParameter("@nameSalt", payment.NameSalt));
                command.Parameters.Add(new MySqlParameter("@amountSalt", payment.AmountSalt));
                command.Parameters.Add(new MySqlParameter("@id", payment.Id));

                command.ExecuteNonQuery();
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
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("UPDATE TRANSACTION " +
                                     "SET AMOUNT = @amount, DESCRIPTION = @description, AMOUNTSALT = @amountSalt, DESCRIPTIONSALT = @descriptionSalt " +
                                     "WHERE ID = @id",
                        connection)
                    { CommandType = CommandType.Text };

                transaction.AmountSalt = Hashing.ExtractSalt(Hashing.CreateHash(Convert.ToString(transaction.Amount)));
                transaction.DescriptionSalt = Hashing.ExtractSalt(Hashing.CreateHash(transaction.Description));

                command.Parameters.Add(new MySqlParameter("@amount", Encryption.Instance.EncryptText(transaction.Amount.ToString(), password, transaction.AmountSalt)));
                command.Parameters.Add(new MySqlParameter("@description", Encryption.Instance.EncryptText(transaction.Description, password, transaction.DescriptionSalt)));
                command.Parameters.Add(new MySqlParameter("@amountSalt", transaction.AmountSalt));
                command.Parameters.Add(new MySqlParameter("@descriptionSalt", transaction.DescriptionSalt));
                command.Parameters.Add(new MySqlParameter("@id", transaction.Id));

                command.ExecuteNonQuery();
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
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user (to identify).</param>
        /// <param name="currencyId">The id of the prefferred currency of the user.</param>
        /// <param name="languageId">The id of the prefferred language of the user.</param>
        /// <param name="password">Password used for encryption.</param>
        /// <param name="salt">Salt used for encryption.</param>
        public void ChangeUser(string name, string lastName, string email, int currencyId, int languageId, string password, string salt)
        {
            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand(
                        "UPDATE USER SET NAME = @name, LASTNAME = @lastName, CURRENCY = @currencyId, LANGUAGE = @languageId " +
                        "WHERE EMAIL = @email",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@name", Encryption.Instance.EncryptText(name, password, salt)));
                command.Parameters.Add(new MySqlParameter("@lastName", Encryption.Instance.EncryptText(lastName, password, salt)));
                command.Parameters.Add(new MySqlParameter("@currencyId", currencyId));
                command.Parameters.Add(new MySqlParameter("@languageId", languageId));
                command.Parameters.Add(new MySqlParameter("@email", email));

                command.ExecuteNonQuery();
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
            throw new NotImplementedException();

            try
            {
                MySqlConnection connection = _db.Connection;
                MySqlCommand command =
                    new MySqlCommand("UPDATE USER SET PASSWORD = @newPassword " +
                                     "WHERE EMAIL = @email",
                        connection)
                    { CommandType = CommandType.Text };

                command.Parameters.Add(new MySqlParameter("@newPassword", Hashing.CreateHash(newPassword)));
                command.Parameters.Add(new MySqlParameter("@email", email));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new ChangePasswordException("Password could not be changed.");
            }
        }
    }
}