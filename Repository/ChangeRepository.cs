using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Classes.Language;
using Library.Exceptions;
using Library.Interfaces;

namespace Repository
{
    public class ChangeRepository
    {
        private readonly IChangeContext _context;

        private ChangeRepository()
        {
            _context = new ChangeSqlContext();
        }

        public static ChangeRepository Instance => new ChangeRepository();

        public void ChangeBalance(Balance balance, string name, decimal balanceAmount, string password)
        {
            try
            {
                Balance dummyBalance = new Balance(balance.Id, name, balanceAmount);

                _context.ChangeBalance(dummyBalance, password);

                balance.Name = name;
                balance.BalanceAmount = balanceAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangePayment(IPayment payment, string name, decimal amount, string password)
        {
            try
            {
                Payment dummyPayment = payment as Payment;

                if (dummyPayment == null) throw new NullReferenceException();

                dummyPayment = (Payment)dummyPayment.Clone();

                dummyPayment.Name = name;
                dummyPayment.Amount = amount;

                _context.ChangePayment(dummyPayment, password);

                payment.Name = name;
                payment.Amount = amount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangeTransaction(Transaction transaction, decimal amount, string description, string password)
        {
            try
            {
                Transaction dummyTransaction = new Transaction(transaction.Id, amount, description, transaction.Positive);

                _context.ChangeTransaction(dummyTransaction, password);

                transaction.Amount = amount;
                transaction.Description = description;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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

                if (DataRepository.Instance.Login(email, currentPassword) == null)
                    throw new ChangeUserException(language.GetText(33));

                if (!string.IsNullOrWhiteSpace(newPassword) && newPassword != repeatedPassword)
                    throw new ChangeUserException(language.GetText(41));

                _context.ChangeUser(name, lastName, email, currencyId, languageId, currentPassword, salt);

                user.Name = name;
                user.LastName = lastName;
                user.Currency.Id = currencyId;
                user.LanguageId = languageId;

                if (string.IsNullOrWhiteSpace(newPassword)) return;

                _context.ChangePassword(email, newPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}