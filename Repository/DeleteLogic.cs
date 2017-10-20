using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class DeleteLogic
    {
        private readonly IDeleteContext _context;
        private readonly Database.Database _database;

        public DeleteLogic()
        {
            _database = new Database.Database();
            _context = new DeleteSqlContext(_database);
        }

        public bool DeleteBalance(User user, int id, string password)
        {
            try
            {
                if (user.GetBalance(id) == null) return false;

                _context.DeleteBalance(id);

                user.DeleteBalance(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public bool DeletePayment(User user, int id, string password)
        {
            try
            {
                if (user.GetPayment(id) == null) return false;

                _context.DeletePayment(id);

                user.DeletePayment(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }

        public bool DeleteTransaction(User user, int id, string password)
        {
            try
            {
                if (user.GetTransaction(id) == null) return false;

                Payment payment = user.GetPaymentByTransaction(id) as Payment;

                if (payment == null) return false;

                _context.DeleteTransaction(id);

                payment.DeleteTransaction(id);

                new BalanceHistoryLogic(_database).UpdateBalance(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;
        }
    }
}