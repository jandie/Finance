using System;
using Database;
using Database.Interfaces;
using Library.Classes;
using Library.Exceptions;

namespace Repository
{
    public class DataRepository
    {
        private static DataRepository _instance;
        private readonly IDataContext _context;

        public static DataRepository Instance => _instance ?? (_instance = new DataRepository());

        public DataRepository()
        {
            _context = new DataSqlContext();
        }

        public User Login(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions)
        {
            try
            {
                return _context.LoginUser(email, password, loadBankAccounts, loadPayments, loadTransactions);
            }
            catch (Exception)
            {
                throw new WrongUsernameOrPasswordException("Wrong username or password!");
            }
        }

        public User CreateUser(string password, string name, string lastName, string email)
        {
            throw new NotImplementedException();
        }
    }
}