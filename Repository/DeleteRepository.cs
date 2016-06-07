using Database;
using Database.Interfaces;

namespace Repository
{
    public class DeleteRepository
    {
        private static DeleteRepository _instance;
        private readonly IDeleteContext _context;

        public static DeleteRepository Instance => _instance ?? (_instance = new DeleteRepository());

        public DeleteRepository()
        {
            _context = new DeleteSqlContext();
        }

        public void DeleteBankAccount(int bankAccountId)
        {
            
        }

        public void DeletePayment(int paymentId)
        {
            
        }

        public void DeleteTransaction(int transactionId)
        {
            
        }
    }
}
