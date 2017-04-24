using System;
using Database.Interfaces;
using Database.SqlContexts;
using Library.Classes;

namespace Repository
{
    public class BalanceHistoryLogic
    {
        private IBalanceHistoryContext _context;

        public void UpdateBalance(User user)
        {
            try
            {
                _context = new BalanceHistorySqlContext();

                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                (_context as IDatabaseClosable)?.CloseDb();
            }
        }
    }
}
