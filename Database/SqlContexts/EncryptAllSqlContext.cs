using System.Collections.Generic;
using Library.Classes;
using Library.Interfaces;

namespace Database.SqlContexts
{
    public class EncryptAllSqlContext
    {
        private readonly DataSqlContext _dataContext;
        private readonly ChangeSqlContext _changeContext;

        public EncryptAllSqlContext()
        {
            _dataContext = new DataSqlContext();
            _changeContext =  new ChangeSqlContext();
        }

        public void EncryptUserData(User user, string oldPassword, string newPassword)
        {
            EncryptBankAccountData(user.Id, oldPassword, newPassword, user.Salt);
            EncryptPaymentData(user.Id, oldPassword, newPassword, user.Salt);
            
            _changeContext.ChangeUser(user, newPassword);
            _changeContext.ChangePassword(user.Email, newPassword);
        }

        private void EncryptBankAccountData(int userId, string oldPassword, string newPassword, string salt)
        {
            List<Balance> bankAccounts = new List<Balance>();

            _dataContext.GetBalancesOfUser(userId, oldPassword, salt).ForEach(b => bankAccounts.Add(b));

            foreach (Balance balance in bankAccounts)
            {
                _changeContext.ChangeBalance(balance, newPassword);
            }
        }

        private void EncryptPaymentData(int userId, string oldPassword, string newPassword, string salt)
        {
            List<IPayment> payments = new List<IPayment>();

            _dataContext.GetPaymentsOfUser(userId, oldPassword, salt).ForEach(p => payments.Add(p));

            foreach (IPayment payment in payments)
            {
                _changeContext.ChangePayment(payment as Payment, newPassword);

                EncryptTransactions(payment, oldPassword, newPassword, salt);
            }
        }

        private void EncryptTransactions(IPayment payment, string oldPassword, string newPassword, string salt)
        {
            List<Transaction> transactions = new List<Transaction>();

            _dataContext.GetTransactionsOfPayment(payment, oldPassword, salt).ForEach(t => transactions.Add(t));

            foreach (Transaction transaction in transactions)
            {
                _changeContext.ChangeTransaction(transaction, newPassword);   
            }
        }
    }
}
