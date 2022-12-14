using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;

namespace FirebirdLiteQuery
{

    public class Executor
    {
        public static IEnumerable ExecuteReader(string commandText)
        {
            FbConnection connection = Connector.GetConnection();
            if (connection == null)
            {
                yield return JsonConvert.SerializeObject(new Dictionary<string, object>() { { "error", "failed connection" } });
                yield break;
            }
            FbTransaction transaction = connection.BeginTransaction();
            FbCommand command = new FbCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = commandText;

            FbDataReader dr = null;
            string error = null;
            try
            {
                dr = command.ExecuteReader();
            } 
            catch (Exception ex)
            {
                transaction.Rollback();                    
                error = JsonConvert.SerializeObject(new Dictionary<string, object> () { { "error", ex.Message } });
            }

            if (error != null)
            {
                yield return error;
                yield break;
            }


            while (dr.Read())
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                for (int i = 0; i< dr.FieldCount; i++)
                {
                    string fieldName = dr.GetName(i).ToLower();
                    string fieldType = dr.GetValue(i).GetType().Name;
                    switch (fieldType)
                    {
                        case "Int16":
                            values.Add(fieldName, dr.GetInt16(i));
                            break;
                        case "Int32":
                            values.Add(fieldName, dr.GetInt32(i));
                            break;
                        case "Int64":
                            values.Add(fieldName, dr.GetInt64(i));
                            break;
                        case "String":
                            values.Add(fieldName, dr.GetString(i));
                            break;
                        case "Byte[]":
                            values.Add(fieldName, Convert.ToBase64String((byte[])dr.GetValue(i)));
                            break;
                        case "Decimal":
                            values.Add(fieldName, dr.GetDecimal(i));
                            break;
                        case "DateTime":
                            values.Add(fieldName, dr.GetDateTime(i));
                            break;
                    }
                    
                }
                yield return JsonConvert.SerializeObject(values);

            }

            transaction.Commit();

        }

        public static string ExecuteNonQuery(string commandText)
        {
            FbConnection connection = Connector.GetConnection();
            if (connection == null)
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>() { { "error", "failed connection" } });
               
            }
            FbTransaction transaction = connection.BeginTransaction();
            FbCommand command = new FbCommand();
            command.Connection = connection;
            command.Transaction = transaction;

            command.CommandText = commandText;

            string error = null;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                error = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "error", ex.Message } });
            }

            if (error != null)
            {
                return error;
                
            }
            transaction.Commit();
            return JsonConvert.SerializeObject(new Dictionary<string, object>() { { "result", "OK" } });
        }

        public static string ExecuteNonQuery(string commandText, Dictionary<string, byte[]> parameters)
        {
            FbConnection connection = Connector.GetConnection();
            if (connection == null)
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>() { { "error", "failed connection" } });

            }
            FbTransaction transaction = connection.BeginTransaction();
            FbCommand command = new FbCommand();
            command.Connection = connection;
            command.Transaction = transaction;


            foreach(KeyValuePair<string, byte[]> parameter in parameters)
            {
                command.Parameters.Add(parameter.Key, FbDbType.Binary);
                command.Parameters[parameter.Key].Value = parameter.Value;
            }
            

            command.CommandText = commandText;

            string error = null;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                error = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "error", ex.Message } });
            }

            if (error != null)
            {
                return error;

            }
            transaction.Commit();
            return JsonConvert.SerializeObject(new Dictionary<string, object>() { { "result", "OK" } });
        }

        public static Dictionary<string, byte[]> CreateParameters()
        {
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
            return result;
        }


    }
}
