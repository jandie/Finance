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

        public EncryptAllSqlContext()
        {
            _dataContext = new DataSqlContext();
            _changeContext =  new ChangeSqlContext();
            _deleteSqlContext = new DeleteSqlContext();
        }

        private void CloseDb()
        {
            (_dataContext as IDatabaseClosable)?.CloseDb();
            (_changeContext as IDatabaseClosable)?.CloseDb();
            (_deleteSqlContext as IDatabaseClosable)?.CloseDb();
        }

        public void EncryptUserData(User user, string oldPassword, string newPassword)
        {
            _deleteSqlContext.DeactivateUser(user.Id);

            EncryptBankAccountData(user.Id, oldPassword, newPassword, user.Salt);
            EncryptPaymentData(user.Id, oldPassword, newPassword, user.Salt);
            
            _changeContext.ChangeUser(user, newPassword);
            _changeContext.ChangePassword(user.Email, newPassword);

            _deleteSqlContext.ActivateUser(user.Id);

            CloseDb();
        }

        private void EncryptBankAccountData(int userId, string oldPassword, string newPassword, string salt)
        {
            List<Balance> bankAccounts = _dataContext.GetBalancesOfUser(userId, oldPassword, salt);

            foreach (Balance balance in bankAccounts)
            {
                _changeContext.ChangeBalance(balance, newPassword);
            }
        }

        private void EncryptPaymentData(int userId, string oldPassword, string newPassword, string salt)
        {
            List<IPayment> payments = _dataContext.GetPaymentsOfUser(userId, oldPassword, salt);

            foreach (IPayment payment in payments)
            {
                _changeContext.ChangePayment(payment as Payment, newPassword);

                EncryptTransactions(payment, oldPassword, newPassword, salt);
            }
        }

        private void EncryptTransactions(IPayment payment, string oldPassword, string newPassword, string salt)
        {
            List<Transaction> transactions = _dataContext.GetTransactionsOfPayment(payment, oldPassword, salt);

            foreach (Transaction transaction in transactions)
            {
                _changeContext.ChangeTransaction(transaction, newPassword);   
            }
        }
    }
}
