using Database.Interfaces;

namespace Database.SqlContexts
{
    public class BalanceHistorySqlContext : IBalanceHistoryContext, IDatabaseClosable
    {
        private readonly Database _db;
        private readonly Encryption _encryption;

        public BalanceHistorySqlContext()
        {
            _db = new Database();
            _encryption = new Encryption();
        }

        public void CloseDb()
        {
            _db.Close();
        }
    }
}
