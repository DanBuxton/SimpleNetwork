using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

            SocketMethod(tcpListener.AcceptSocket());
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private void SocketMethod(Socket socket)
        {
            string receivedMessage;
            NetworkStream stream = new NetworkStream(socket);

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.WriteLine();
            writer.Flush();

            while ((receivedMessage = Console.ReadLine()) != null && receivedMessage.ToLower() != "end")
            {
                string msg = GetReturnMessage(receivedMessage);

                writer.WriteLine(msg);
                writer.Flush();
            }

            socket.Close();
        }

        private string GetReturnMessage(string code)
        {
            return "";
        }
    }
}
