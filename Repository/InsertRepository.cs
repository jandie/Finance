using Database;
using Database.Interfaces;
using Library.Classes;
using Library.Enums;

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

        }

        public void AddPayment(int userId, string name, decimal amount, PaymentType type)
        {
            
        }

        public void AddTransaction(Payment payment, decimal amount, string description)
        {
            
        }
    }
}
