using System;
using System.Data;
using Database.Properties;
using MySql.Data.MySqlClient;

namespace Database
{
    public class Database
    {
        /// <summary>
        /// Creates a new instance of the database connection.
        /// </summary>
        public Database()
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

        /// <summary>
        /// The database connection.
        /// </summary>
        public MySqlConnection Connection { get; }

        /// <summary>
        /// Closes the conneciton with the database.
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Destructor that closes the connection with the database.
        /// </summary>
        ~Database()
        {
            Connection.Close();
        }
    }
}