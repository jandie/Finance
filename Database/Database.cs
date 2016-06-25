using System.Data;
using MySql.Data.MySqlClient;

namespace Database
{
    public class Database
    {
        private static readonly string CONNECTION_STRING =
            "SERVER=" + DatabaseConfiguration.DatabaseHost + ";" + "DATABASE=" +
            DatabaseConfiguration.DatabaseName + ";" + "UID=" + DatabaseConfiguration.DatabaseUsername + ";" + "PASSWORD=" + DatabaseConfiguration.DatabasePassword + ";";

        private static Database _instance;

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

        private bool IsConnected => Connection.State == ConnectionState.Open;

        public MySqlConnection Connection { get; }

        private Database()
        {
            Connection = new MySqlConnection { ConnectionString = CONNECTION_STRING };

            Connection.Open();
        }

        ~Database()
        {
            Connection.Close();
        }
    }
}