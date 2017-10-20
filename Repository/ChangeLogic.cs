using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Classes.Language;
using Library.Exceptions;
using Library.Interfaces;

namespace Repository
{
    public class ChangeLogic
    {
        private readonly IChangeContext _context;
        private readonly Database.Database _database;

        public ChangeLogic()
        {
            _database = new Database.Database();
            _context = new ChangeSqlContext(_database);
        }

        public bool ChangeBalance(User user, int balanceId, string name, 
            decimal balanceAmount, string password)
        {
            try
            {
                Balance balance = user.GetBalance(balanceId);

                if (balance == null) return false;

                Balance dummyBalance = new Balance(balanceId, name, balanceAmount);

                _context.ChangeBalance(dummyBalance, password);

                balance.Name = name;
                balance.BalanceAmount = balanceAmount;

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public bool ChangePayment(User user, int paymentId, string name, decimal amount, string password)
        {
            try
            {
                IPayment payment = user.GetPayment(paymentId);
                Payment dummyPayment = payment as Payment;

                if (dummyPayment == null) return false;

                dummyPayment = (Payment)dummyPayment.Clone();

                dummyPayment.Name = name;
                dummyPayment.Amount = amount;

                _context.ChangePayment(dummyPayment, password);

                payment.Name = name;
                payment.Amount = amount;

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public bool ChangeTransaction(User user, int transactionId, decimal amount, string description, string password)
        {
            try
            {
                Transaction transaction = user.GetTransaction(transactionId);

                if (transaction == null) return false;

                Transaction dummyTransaction = new Transaction(transaction.Id, amount, description, transaction.Positive);

                _context.ChangeTransaction(dummyTransaction, password);

                transaction.Amount = amount;
                transaction.Description = description;

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public void ChangeUser(User user, string name, string lastName, string email, int currencyId, int languageId,
            string currentPassword, string newPassword, string repeatedPassword, Language language, string salt)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ChangeUserException(language.GetText(35));

                if (string.IsNullOrWhiteSpace(lastName))
                    throw new ChangeUserException(language.GetText(36));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Length < 8)
                    throw new ChangeUserException(language.GetText(39));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Contains(" "))
                    throw new ChangeUserException(language.GetText(40));

                if (new DataLogic().Login(email, currentPassword) == null)
                    throw new ChangeUserException(language.GetText(33));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != repeatedPassword)
                    throw new ChangeUserException(language.GetText(41));

                user.Name = name;
                user.LastName = lastName;
                user.Currency.Id = currencyId;
                user.LanguageId = languageId;

                _context.ChangeUser(user, currentPassword);

                if (string.IsNullOrWhiteSpace(newPassword)) return;

                new EncryptAllSqlContext(_database).EncryptUserData(user, currentPassword, newPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}