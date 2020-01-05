using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //new SimpleClient().Connect("127.0.0.1", 4444);

            //Task.Factory.StartNew(() => new SimpleClient());
            Task.Factory.StartNew(() => new SimpleClient());
            Task.Factory.StartNew(() => new SimpleClient());

            new SimpleClient();

            //for (int i = 0; i < 3; i++)
            //{
            //    Debugger.Log(1, null, "In Loop\n");
            //    Task.Run(() => new SimpleClient());
            //}

            //new SimpleClient().Run();
        }
    }
}
