using Library.Classes;

namespace Database.Interfaces
{
    public interface IBalanceHistoryContext
    {
        BalanceHistory UpdateBalanceHistory(User user, string password);
    }
}
