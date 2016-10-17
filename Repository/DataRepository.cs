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
    public class DataRepository
    {
        private static DataRepository _instance;
        private readonly IDataContext _context;

        private DataRepository()
        {
            _context = new DataSqlContext();
        }

        public static DataRepository Instance => _instance ?? (_instance = new DataRepository());

        public User Login(string email, string password)
        {
            try
            {
                return _context.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

        public User CheckUser(User user)
        {
            try
            {
                return _context.TokenChanged(user.Email, user.Token) ? null : user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

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

                if (alphaKey != "E1j6kr!v4")
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