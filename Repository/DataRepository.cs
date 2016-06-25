using System;
using Database;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;
using Library.Exceptions;

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
                return null;
            }
        }

        public User CreateUser(string name, string lastName, string email, string password)
        {
            try
            {
                return _context.CreateUser(name, lastName, email, password);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}