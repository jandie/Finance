using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class DeleteLogic
    {
        private readonly IDeleteContext _context;

        public DeleteLogic()
        {
            _context = new DeleteSqlContext();
        }

        public bool DeleteBalance(User user, int id)
        {
            try
            {
                if (user.GetBalance(id) == null) return false;

                _context.DeleteBalance(id);

                user.DeleteBalance(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                _context.CloseDb();
            }

            return true;
        }

        public bool DeletePayment(User user, int id)
        {
            try
            {
                if (user.GetPayment(id) == null) return false;

                _context.DeletePayment(id);

                user.DeletePayment(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                _context.CloseDb();
            }

            return true;
        }

        public bool DeleteTransaction(User user, int id)
        {
            try
            {
                if (user.GetTransaction(id) == null) return false;

                Payment payment = user.GetPaymentByTransaction(id) as Payment;

                if (payment == null) return false;

                _context.DeleteTransaction(id);

                payment.DeleteTransaction(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                _context.CloseDb();
            }

            return true;
        }
    }
}