using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";

            SimpleClient sc = new SimpleClient();

            //Task.Run(() => sc.Connect("127.0.0.1", 4444));

            sc.Connect("127.0.0.1", 4444);
        }
    }
}
