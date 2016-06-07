using System;
using Database;
using Database.Interfaces;
using Library.Classes;
using Library.Enums;
using Library.Interfaces;

namespace Repository
{
    public class InsertRepository
    {
        private static InsertRepository _instance;
        private readonly IInsertContext _context;
        public static InsertRepository Instance => _instance ?? (_instance = new InsertRepository());

        public InsertRepository()
        {
            _context = new InsertSqlContext();
        }
        public void AddBankAccount(int userId, string name, decimal balance)
        {
            try
            {
                _context.AddBankAccount(userId, name, balance);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void AddPayment(int userId, string name, decimal amount, PaymentType type)
        {
            try
            {
                _context.AddPayment(userId, name, amount, type);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void AddTransaction(IPayment payment, decimal amount, string description)
        {
            try
            {
                if (payment.MayAddPayment) _context.AddTransaction(payment.Id, amount, description);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
