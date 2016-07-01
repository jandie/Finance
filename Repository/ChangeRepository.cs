using System;
using Database.Interfaces;
using Database.SqlContexts;

namespace Repository
{
    public class ChangeRepository
    {
        private static ChangeRepository _instance;
        private readonly IChangeContext _context;

        public static ChangeRepository Instance => _instance ?? (_instance = new ChangeRepository());

        public ChangeRepository()
        {
            _context = new ChangeSqlContext();
        }

        public void ChangeBalance(int id, string name, decimal balanceAmount)
        {
            try
            {
                _context.ChangeBalance(id, name, balanceAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangePayment(int id, string name, decimal amount)
        {
            try
            {
                _context.ChangePayment(id, name, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangeTransaction(int id, decimal amount, string description)
        {
            try
            {
                _context.ChangeTransaction(id, amount, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangeUser(string name, string lastName, string email, int currencyId, int languageId,
            string currentPassword)
        {
            try
            {
                _context.ChangeUser(name, lastName, email, currencyId, languageId, currentPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ChangePassword(string email, string newPassword, string currentPassword)
        {
            try
            {
                _context.ChangePassword(email, newPassword, currentPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
