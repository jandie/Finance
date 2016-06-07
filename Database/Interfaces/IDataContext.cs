using Library.Classes;

namespace Database.Interfaces
{
    public interface IDataContext
    {
        User LoginUser(string email, string password, bool loadBankAccounts, bool loadPayments, bool loadTransactions);
    }
}
