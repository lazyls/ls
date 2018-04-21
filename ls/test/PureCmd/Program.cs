using ls.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PureCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Browser();
            //client.SetProxy("194.67.201.106:3128");
            string resp = client.Get("http://www.baidu.com");
            client.Timeout = 10;
            Console.WriteLine(resp);

            Console.WriteLine("C");
        }
    }
}
