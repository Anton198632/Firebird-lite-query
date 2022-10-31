using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdSql.Data.FirebirdClient;


namespace FirebirdLiteQuery
{
    public class Connector
    {
        private Connector() { }
        private static FbConnection _connect;
        private static string server;
        private static string dbPath;
        private static string userName;
        private static string password;

        public static FbConnection GetConnection()
        {
            if (_connect == null || _connect.State == System.Data.ConnectionState.Closed)
            {
                FbConnectionStringBuilder stringBuilder = new FbConnectionStringBuilder();
                stringBuilder.DataSource = server;
                stringBuilder.Database = dbPath;
                stringBuilder.UserID = userName;
                stringBuilder.Password = password;
                stringBuilder.Charset = "WIN1251";
                stringBuilder.Dialect = 3;

                

                try
                {
                    _connect = new FbConnection(stringBuilder.ToString());
                    _connect.Open();
                }
                catch (Exception ex)
                {

                    return null;
                }
                
            }
               
            return _connect;
        }

        public static void Connect(string server, string dbPath, string userName = "SYSDBA", string password = "masterkey")
        {
            Connector.server = server;
            Connector.dbPath = dbPath;
            Connector.userName = userName;
            Connector.password = password;

        }



        public static void Disconnect ()
        {
            if (_connect != null)
                _connect.Close();
        }

    }
}
