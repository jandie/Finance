using System;
using System.Collections.Generic;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Classes.Language;
using Library.Exceptions;
using Repository.Utilities;

namespace Repository
{
    public class DataLogic
    {
        private readonly IDataContext _context;
        private readonly Database.Database _database;

        public DataLogic()
        {
            _database = new Database.Database();
            _context = new DataSqlContext(_database);
        }

        /// <summary>
        /// Retrieves the user based on email and password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The mached user.</returns>
        public User Login(string email, string password)
        {
            try
            {
                User user = _context.LoginUser(email, password);

                new BalanceHistoryLogic(_database).GetBalanceHistoryOfUser(user).ForEach(b => user.AddBalanceHistory(b));

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Checks id the users token has'nt changed. If it was changed the user is refreshed.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password used for encryption.</param>
        /// <returns></returns>
        public User CheckUser(User user, string password)
        {
            try
            {
                return _context.TokenChanged(user.Email, user.Token) ? Login(user.Email, password) : user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="lastName">The lastname of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="password2">The repeated password of the user.</param>
        /// <param name="currencyId">The currency id preference.</param>
        /// <param name="languageId">The language id preference.</param>
        /// <param name="language">The language for error messages.</param>
        /// <param name="alphaKey">The alpha key for alpha users.</param>
        /// <returns></returns>
        public User CreateUser(string name, string lastName, string email, string password, string password2,
            int currencyId, int languageId,
            Language language, string alphaKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new RegistrationException(language.GetText(35));

                if (string.IsNullOrWhiteSpace(lastName))
                    throw new RegistrationException(language.GetText(36));

                if (string.IsNullOrWhiteSpace(email))
                    throw new RegistrationException(language.GetText(37));

                if (!RegexUtilities.Instance.IsValidEmail(email))
                    throw new RegistrationException(language.GetText(38));

                if (password.Length < 8)
                    throw new RegistrationException(language.GetText(39));

                if (password.Contains(" "))
                    throw new RegistrationException(language.GetText(40));

                if (password != password2)
                    throw new RegistrationException(language.GetText(41));

                if (alphaKey != "hendriks")
                    throw new RegistrationException(
                        "Because this website is still in alpha, you need a key to be able to register.");

                User user = _context.CreateUser(name, lastName, email, password, currencyId, languageId);

                if (user == null)
                {
                    throw new RegistrationException(language.GetText(42));
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }

        /// <summary>
        /// Gets all available languages.
        /// </summary>
        /// <returns></returns>
        public List<Language> LoadLanguages()
        {
            try
            {
                return _context.LoadLanguages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }

        /// <summary>
        /// Gets all available currencies.
        /// </summary>
        /// <returns></returns>
        public List<Currency> LoadCurrencies()
        {
            try
            {
                return _context.LoadCurrencies();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}