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
            for(int i = 0; i < 1000; i++)
                foreach (dynamic data in Executor.ExecuteReader("select * from USERS"))
                {
                    Console.WriteLine(data);
                }

            Console.WriteLine(Executor.ExecuteNonQuery("insert into USERS (USERNAME) values ('Mary')"));

            Console.Read();

        }
    }
}
