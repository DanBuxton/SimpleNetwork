using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class SimpleServer
    {
        private TcpListener tcpListener { get; set; }

        public SimpleServer(string ipAddress, int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public void Start()
        {
            tcpListener.Start();
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private void SocketMethod(Socket socket)
        {

        }

        private string GetReturnMessage(string code)
        {
            return "";
        }
    }
}
