using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenerateCodeByDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = ConfigurationManager.ConnectionStrings["CanadaComputersERP"].ConnectionString;
            Console.WriteLine(a);
        }
    }
}
