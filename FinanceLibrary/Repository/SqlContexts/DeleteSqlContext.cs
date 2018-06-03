﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using FinanceLibrary.Exceptions;
using FinanceLibrary.Repository.Interfaces;

namespace FinanceLibrary.Repository.SqlContexts
{
    public class DeleteSqlContext : IDeleteContext
    {
        private readonly Database _db;

        public DeleteSqlContext(Database database)
        {
            _db = database;
        }

        /// <summary>
        /// Deactivates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        public void DeactivateUser(int id)
        {
            ChangeUserActiveStatus(id, 0);
        }

        /// <summary>
        /// Activates user in the database.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        public void ActivateUser(int id)
        {
            ChangeUserActiveStatus(id, 1);
        }

        /// <summary>
        /// Changes the status of the user to active (1) or not inactive(0).
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="status">The new status of the user.</param>
        private void ChangeUserActiveStatus(int id, int status)
        {
            try
            {
                const string query = "UPDATE USER SET ACTIVE = @status WHERE ID = @id";
                
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"status", status},
                    {"id", id}
                };

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeleteBalanceException("User could not be (de)activated.");
            }
        }

        /// <summary>
        /// Deactivates a balance in the database.
        /// </summary>
        /// <param name="id">The id of the balance.</param>
        public void DeleteBalance(int id)
        {
            try
            {
                const string query = "DELETE FROM BANKACCOUNT WHERE ID = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>{{"id", id}};

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                
                throw new DeleteBalanceException("Balance could not be deleted.");
            }
        }

        /// <summary>
        /// Deactivates a payment in the database.
        /// </summary>
        /// <param name="id">The id of the payment.</param>
        public void DeletePayment(int id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "id", id } };

                _db.Execute("DELETE FROM TRANSACTION WHERE Payment_Id = @id",
                    parameters, Database.QueryType.NonQuery);
                _db.Execute("DELETE FROM PAYMENT WHERE ID = @id", 
                    parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeletePaymentException("Payment could not be deleted.");
            }
        }

        /// <summary>
        /// Deactivates a transaction in the database.
        /// </summary>
        /// <param name="id">The id of the transaction.</param>
        public void DeleteTransaction(int id)
        {
            try
            {
                const string query = "DELETE FROM TRANSACTION WHERE ID = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>{{"id", id}};

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeleteTransactionException("Transaction could not be deleted.");
            }
        }

        /// <summary>
        /// Completely removes user from the database.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        public void DeleteUser(string email)
        {
            DeleteTransactionsFromUser(email);
            DeletePaymentsFromUser(email);
            DeleteBalancesFromUser(email);

            const string query = "DELETE FROM USER WHERE email = @email";

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "email", email } };

            _db.Execute(query, parameters, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Removes all transactions of a user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        private void DeleteTransactionsFromUser(string email)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "email", email } };

            _db.Execute("DELETE t FROM TRANSACTION t INNER JOIN PAYMENT p ON p.Id = t.Payment_Id INNER JOIN USER u ON u.Id = p.User_Id WHERE u.Email = @email", 
                parameters, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Removes all payments of a user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        private void DeletePaymentsFromUser(string email)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "email", email } };

            _db.Execute("DELETE p FROM PAYMENT p INNER JOIN USER u ON u.Id = p.User_Id WHERE u.Email = @email", 
                parameters, Database.QueryType.NonQuery);
        }

        /// <summary>
        /// Removes all balances & balance history of a user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        private void DeleteBalancesFromUser(string email)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "email", email } };

            _db.Execute("DELETE bh FROM BALANCEHISTORY bh INNER JOIN USER u ON u.Id = bh.UserId WHERE u.Email = @email", 
                parameters, Database.QueryType.NonQuery);
            _db.Execute("DELETE ba FROM BANKACCOUNT ba INNER JOIN USER u ON u.Id = ba.User_Id WHERE u.Email = @email",
                parameters, Database.QueryType.NonQuery);
        }
    }
}