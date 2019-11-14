using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task.Run(() => sc.Connect("127.0.0.1", 4444));

            new SimpleClient().Connect("127.0.0.1", 4444);

            //new SimpleClient().Run();
        }
    }
}
