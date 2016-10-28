using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class DeleteRepository
    {
        private static DeleteRepository _instance;
        private readonly IDeleteContext _context;

        private DeleteRepository()
        {
            _context = new DeleteSqlContext();
        }

        public static DeleteRepository Instance => _instance ?? (_instance = new DeleteRepository());

        public void DeleteBalance(User user, int id)
        {
            try
            {
                _context.DeleteBalance(id);

                user.DeleteBalance(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void DeletePayment(User user, int id)
        {
            try
            {
                _context.DeletePayment(id);

                user.DeletePayment(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void DeleteTransaction(Payment payment, int id)
        {
            try
            {
                _context.DeleteTransaction(id);

                payment.DeleteTransaction(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}