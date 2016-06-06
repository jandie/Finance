using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Database
{
    public class Database
    {
        private static readonly string CONNECTION_STRING =
            $"data source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = {DatabaseConfiguration.DatabaseHost})(PORT = {DatabaseConfiguration.DatabasePort})))(CONNECT_DATA =(SERVICE_NAME = {DatabaseConfiguration.DatabaseService})));USER ID={DatabaseConfiguration.DatabaseUsername};PASSWORD={DatabaseConfiguration.DatabasePassword}";

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

        public OracleConnection Connection { get; }

        private Database()
        {
            Connection = new OracleConnection { ConnectionString = CONNECTION_STRING };

            Open();
        }

        ~Database()
        {
            Close();
        }

        private void Open()
        {
            Connection.Open();
        }

        private void Close()
        {
            Connection.Close();
        }
    }
}