using System.Data;
using Database.Properties;
using MySql.Data.MySqlClient;

namespace Database
{
    public class Database
    {
        private static Database _instance;

        private Database()
        {
            string connectionString =
                $"SERVER={Settings.Default.DatabaseHost};" +
                $"PORT={Settings.Default.DatabasePort};" +
                $"DATABASE={Settings.Default.DatabaseName};" +
                $"UID={Settings.Default.DatabaseUsername};" +
                $"PASSWORD={Settings.Default.DatabasePassword};";

            Connection = new MySqlConnection {ConnectionString = connectionString};

            Connection.Open();
        }

        private bool IsConnected => Connection.State == ConnectionState.Open;

        public MySqlConnection Connection { get; }

        public static Database Instance
        {
            get
            {
                if (_instance == null || !_instance.IsConnected)
                {
                    return _instance = new Database();
                }

                return _instance;
            }
        }

        ~Database()
        {
            Connection.Close();
        }
    }
}