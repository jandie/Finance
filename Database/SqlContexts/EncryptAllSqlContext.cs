using System.Collections.Generic;
using Database.Interfaces;
using Library.Classes;
using Library.Interfaces;

namespace Database.SqlContexts
{
    public class EncryptAllSqlContext
    {
        private readonly IDataContext _dataContext;
        private readonly IChangeContext _changeContext;
        private readonly IDeleteContext _deleteSqlContext;

        public EncryptAllSqlContext(Database database)
        {
            _dataContext = new DataSqlContext(database);
            _changeContext =  new ChangeSqlContext(database);
            _deleteSqlContext = new DeleteSqlContext(database);
        }

        /// <summary>
        /// Encrypts all data of a user with the new password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="oldPassword">The old password of the user.</param>
        /// <param name="newPassword">The new password of the user.</param>
        public void EncryptUserData(User user, string oldPassword, string newPassword)
        {
            _deleteSqlContext.DeactivateUser(user.Id);

            EncryptBankAccountData(user.Id, oldPassword, newPassword);
            EncryptPaymentData(user.Id, oldPassword, newPassword);
            
            _changeContext.ChangeUser(user);
            _changeContext.ChangePassword(user, newPassword);

            _deleteSqlContext.ActivateUser(user.Id);
        }

        /// <summary>
        /// Encrypts all bank account of a user with the new password.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldPassword">The old password of the user.</param>
        /// <param name="newPassword">The new password of the user.</param>
        private void EncryptBankAccountData(int userId, string oldPassword, string newPassword)
        {
            List<Balance> bankAccounts = _dataContext.GetBalancesOfUser(userId, oldPassword);

            foreach (Balance balance in bankAccounts)
            {
                _changeContext.ChangeBalance(balance, newPassword);
            }
        }

        /// <summary>
        /// Encrypts all payment data of a user with the new password.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldPassword">The old password of the user.</param>
        /// <param name="salt">The salt used for encryption.</param>
        private void EncryptPaymentData(int userId, string oldPassword, string newPassword)
        {
            List<IPayment> payments = _dataContext.GetPaymentsOfUser(userId, oldPassword);

            foreach (IPayment payment in payments)
            {
                _changeContext.ChangePayment(payment as Payment, newPassword);

                EncryptTransactions(payment, oldPassword, newPassword);
            }
        }

        /// <summary>
        /// Encrypts all transaction data of a payment with the new password.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="oldPassword">The old password of the user.</param>
        /// <param name="newPassword">The new password of the user.</param>
        private void EncryptTransactions(IPayment payment, string oldPassword, string newPassword)
        {
            List<Transaction> transactions = _dataContext.GetTransactionsOfPayment(payment, oldPassword);

            foreach (Transaction transaction in transactions)
            {
                _changeContext.ChangeTransaction(transaction, newPassword);   
            }
        }
    }
}
