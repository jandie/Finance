using System;
using System.Collections.Generic;
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
        /// The connection to the database.
        /// </summary>
        private MySqlConnection Connection { get; }

        /// <summary>
        /// Opens a MySQL connection
        /// </summary>
        /// <param name="con">The MySQL connection.</param>
        private void OpenConnection(MySqlConnection con)
        {
            if (con?.State != System.Data.ConnectionState.Open)
            {
                con?.Open();
            }
        }

        /// <summary>
        /// Closes a MySQL connection.
        /// </summary>
        /// <param name="con">The MySQL connection.</param>
        private void CloseConnection(MySqlConnection con)
        {
            if (con?.State == System.Data.ConnectionState.Open)
            {
                con?.Close();
            }
        }

        /// <summary>
        /// Executes a query with the MySQL connection string and returns data based on the querytype.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="queryType">The type of query.</param>
        /// <returns>Returned data from the query.</returns>
        public object Execute(string query, Dictionary<string, object> parameters, QueryType queryType)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            try
            {
                OpenConnection(Connection);

                switch (queryType)
                {
                    case QueryType.NonQuery:
                        ExecuteNoReturn(query, parameters);
                        break;

                    case QueryType.Return:
                        return ExecuteReturn(query, parameters);

                    case QueryType.DataSet:
                        return ExecuteReturnDataset(query, parameters);

                    case QueryType.Insert:
                        return ExecuteInsert(query, parameters);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(queryType), queryType, null);
                }
            }
            finally
            {
                CloseConnection(Connection);
            }

            return null;
        }

        /// <summary>
        /// Executes a query with the MySQL connection string and returns a DataSet.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>Returned data from the query.</returns>
        private DataSet ExecuteReturnDataset(string query, Dictionary<string, object> parameters)
        {
            DataSet ds = new DataSet();

            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                ApplyParameters(cmd, parameters);

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(ds);

                    return ds;
                }
            }
        }

        /// <summary>
        /// Executes a query with the MySQL connection string and returns a DataTable.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>Returned data from the query.</returns>
        private DataTable ExecuteReturn(string query, Dictionary<string, object> parameters)
        {
            DataTable results = new DataTable("Results");

            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                ApplyParameters(cmd, parameters);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    results.Load(reader);

                return results;
            }
        }

        /// <summary>
        /// Executes a query with the MySQL connection string.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters for the query.</param>
        private void ExecuteNoReturn(string query, Dictionary<string, object> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                ApplyParameters(cmd, parameters);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a insert query and returns the last inserted ID.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns></returns>
        private long ExecuteInsert(string query, Dictionary<string, object> parameters)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                ApplyParameters(cmd, parameters);

                cmd.ExecuteNonQuery();

                return cmd.LastInsertedId;
            }
        }

        /// <summary>
        /// Applies the parameters to the MySqlCommand.
        /// </summary>
        /// <param name="cmd">The MySqlCommand object.</param>
        /// <param name="parameters">The parameters to apply.</param>
        private void ApplyParameters(MySqlCommand cmd, Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                cmd.Parameters.Add(new MySqlParameter(parameter.Key, parameter.Value));
            }
        }

        /// <summary>
        /// The different type of queries.
        /// </summary>
        public enum QueryType
        {
            NonQuery,
            Return,
            DataSet,
            Insert
        }
    }
}