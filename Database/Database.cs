using System;
using System.Data;
using Database.Properties;
using MySql.Data.MySqlClient;

namespace Database
{
    public class Database
    {
        private Database()
        {
            string password = Environment.GetEnvironmentVariable("FINANCEDBPWD", 
                EnvironmentVariableTarget.Machine);

            string connectionString =
                $"SERVER={Settings.Default.DatabaseHost};" +
                $"PORT={Settings.Default.DatabasePort};" +
                $"DATABASE={Settings.Default.DatabaseName};" +
                $"UID={Settings.Default.DatabaseUsername};" +
                $"PASSWORD={password};";

            Connection = new MySqlConnection {ConnectionString = connectionString};

            Connection.Open();
        }

        private bool IsConnected => Connection.State == ConnectionState.Open;

        public MySqlConnection Connection { get; }

        public static Database NewInstance => new Database();

        ~Database()
        {
            Connection.Close();
        }
    }
}