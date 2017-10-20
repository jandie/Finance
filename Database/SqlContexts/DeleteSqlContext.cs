using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database.Interfaces;
using Library.Exceptions;

namespace Database.SqlContexts
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
                const string query = "UPDATE BANKACCOUNT SET ACTIVE = 0 WHERE ID = @id";
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
                const string query = "UPDATE PAYMENT SET ACTIVE = 0 WHERE ID = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>{{"id", id}};

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
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
                const string query = "UPDATE TRANSACTION SET ACTIVE = 0 WHERE ID = @id";
                Dictionary<string, object> parameters = new Dictionary<string, object>{{"id", id}};

                _db.Execute(query, parameters, Database.QueryType.NonQuery);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);

                throw new DeleteTransactionException("Transaction could not be deleted.");
            }
        }
    }
}