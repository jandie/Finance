using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Enums;

namespace Repository
{
    public class InsertRepository
    {
        private static InsertRepository _instance;
        private readonly IInsertContext _context;

        public InsertRepository()
        {
            _context = new InsertSqlContext();
        }

        public static InsertRepository Instance => _instance ?? (_instance = new InsertRepository());

        public void AddBankAccount(int userId, string name, decimal balance)
        {
            try
            {
                _context.AddBankAccount(userId, name, balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddPayment(int userId, string name, decimal amount, PaymentType type)
        {
            try
            {
                _context.AddPayment(userId, name, amount, type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void AddTransaction(int paymentId, decimal amount, string description)
        {
            try
            {
                _context.AddTransaction(paymentId, amount, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}