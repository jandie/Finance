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

        /// <summary>
        /// Changes a Balance of the User.
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="balanceId">The ID of the Balance of the User.</param>
        /// <param name="name">The name of the Balance.</param>
        /// <param name="balanceAmount">The amount of the balance.</param>
        /// <returns>Whether or not the action was completed successfully.</returns>
        public bool ChangeBalance(User user, int balanceId, string name, 
            decimal balanceAmount)
        {
            try
            {
                Balance balance = user.GetBalance(balanceId);

                if (balance == null) return false;

                Balance dummyBalance = new Balance(balanceId, name, balanceAmount);

                _context.ChangeBalance(dummyBalance, user.MasterPassword);

                balance.Name = name;
                balance.BalanceAmount = balanceAmount;

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// Changes a payment of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="paymentId">The id of the payment of the user.</param>
        /// <param name="name">The name of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <returns>Whether or not the action was completed successfully.</returns>
        public bool ChangePayment(User user, int paymentId, string name, decimal amount)
        {
            try
            {
                IPayment payment = user.GetPayment(paymentId);

                if (!(payment is Payment dummyPayment)) return false;

                dummyPayment = (Payment)dummyPayment.Clone();

                dummyPayment.Name = name;
                dummyPayment.Amount = amount;

                _context.ChangePayment(dummyPayment, user.MasterPassword);

                payment.Name = name;
                payment.Amount = amount;

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// Changes a transaction of the user.
        /// </summary>
        /// <param name="user">The user itself./</param>
        /// <param name="transactionId">The id of the transaction.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="description">The description of the transaction.</param>
        /// <returns>Whether or not the action was completed with success.</returns>
        public bool ChangeTransaction(User user, int transactionId, decimal amount, string description)
        {
            try
            {
                Transaction transaction = user.GetTransaction(transactionId);

                if (transaction == null) return false;

                Transaction dummyTransaction = new Transaction(transaction.Id, amount, description, transaction.Positive);

                _context.ChangeTransaction(dummyTransaction, user.MasterPassword);

                transaction.Amount = amount;
                transaction.Description = description;

                new BalanceHistoryLogic(_database).UpdateBalance(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// Changes the user itself.
        /// </summary>
        /// <param name="user">The user itself.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="currencyId">The currency id of the user.</param>
        /// <param name="languageId">The language id of the user.</param>
        /// <param name="currentPassword">The users current password.</param>
        /// <param name="newPassword">The users new password.</param>
        /// <param name="repeatedPassword">The users current password again.</param>
        /// <param name="language">The users language.</param>
        public void ChangeUser(User user, string name, string lastName, string email, int currencyId, int languageId,
            string currentPassword, string newPassword, string repeatedPassword, Language language)
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

                _context.ChangeUser(user);

                if (string.IsNullOrWhiteSpace(newPassword)) return;

                _context.ChangePassword(user, newPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}