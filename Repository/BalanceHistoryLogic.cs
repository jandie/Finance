using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class BalanceHistoryLogic
    {
        private readonly IBalanceHistoryContext _context;

        public BalanceHistoryLogic()
        {
            _context = new BalanceHistorySqlContext();
        }

        public void UpdateBalance(User user, string password)
        {
            try
            {
                BalanceHistory balanceHistory = _context.UpdateBalanceHistory(user, password);

                user.DeleteBalanceHistory(balanceHistory.Id);
                user.AddBalanceHistory(balanceHistory);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                (_context as IDatabaseClosable)?.CloseDb();
            }
        }
    }
}
