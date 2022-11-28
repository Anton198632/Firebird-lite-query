using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdLiteQuery;

namespace Firebird_lite_query
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Connector.Connect("localhost", @"d:\С#\Firebird lite query\FirebirdLiteQuery\bin\Debug\DATABASE.FDB");
            for (int i = 0; i < 1000; i++)
                foreach (dynamic data in Executor.ExecuteReader("select * from USERS"))
                {
                    Console.WriteLine(data);
                }

            Console.WriteLine(Executor.ExecuteNonQuery("insert into USERS (USERNAME) values ('Mary')"));

            Dictionary<string, byte[]> parameters = new Dictionary<string, byte[]>();
            parameters.Add("IMAGE_DATA", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, });

            Console.WriteLine(Executor.ExecuteNonQuery("insert into USERS (USERNAME, IMAGE) values ('Mary', @IMAGE_DATA)", parameters));

            Console.Read();

        }
    }
}
