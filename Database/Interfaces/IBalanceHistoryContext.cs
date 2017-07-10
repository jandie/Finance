using System.Collections.Generic;
using Library.Classes;

namespace Database.Interfaces
{
    public interface IBalanceHistoryContext
    {
        BalanceHistory UpdateBalanceHistory(User user, string password);

        List<BalanceHistory> GetBalanceHistoriesOfMonth(User user, string monthYear, string password);
    }
}
