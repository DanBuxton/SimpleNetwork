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

            Console.WriteLine("Listening...");

            Socket socket = tcpListener.AcceptSocket();
            Console.WriteLine("Connection Made");

            //Task.Run(()=> SocketMethod(socket));
            SocketMethod(socket);
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private void SocketMethod(Socket socket)
        {
            try
            {
                string receivedMessage = "";
                NetworkStream stream = new NetworkStream(socket, true);

                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                writer.WriteLine("Welcome");
                writer.Flush();

                while ((receivedMessage = Console.ReadLine()) != null && receivedMessage.ToLower() != "exit")
                {
                    string msg = GetReturnMessage(receivedMessage);

                    writer.WriteLine(msg);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                socket.Close();
            }
        }

        private string GetReturnMessage(string code)
        {
            return code;
        }
    }
}
