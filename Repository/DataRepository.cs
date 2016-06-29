using System;
using System.Collections.Generic;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Classes.Language;

namespace Repository
{
    public class DataRepository
    {
        private static DataRepository _instance;
        private readonly IDataContext _context;

        public static DataRepository Instance => _instance ?? (_instance = new DataRepository());

        private DataRepository()
        {
            _context = new DataSqlContext();
        }

        public User Login(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            try
            {
                return _context.LoginUser(email, password, loadBankAccounts, loadPayments, loadTransactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;
            }
        }

        public User CreateUser(string name, string lastName, string email, string password, int currencyId, int languageId)
        {
            try
            {
                return _context.CreateUser(name, lastName, email, password, currencyId, languageId);
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