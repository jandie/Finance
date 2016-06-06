using Database;
using Database.Interfaces;

namespace Repository
{
    public class DataRepository
    {
        private static DataRepository _instance;
        private readonly IDataContext _context;

        public static DataRepository Instance => _instance ?? (_instance = new DataRepository());

        public DataRepository()
        {
            _context = new DataSqlContext();
        }
    }
}