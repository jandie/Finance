using System;
using Database;
using Database.Interfaces;
using Database.SqlContexts;

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

        public void DeleteBalance(int id)
        {
            try
            {
                _context.DeleteBalance(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void DeletePayment(int id)
        {
            try
            {
                _context.DeletePayment(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void DeleteTransaction(int id)
        {
            try
            {
                _context.DeleteTransaction(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
