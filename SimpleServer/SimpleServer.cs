using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SimpleServer
{
    class SimpleServer
    {
        private TcpListener tcpListener { get; set; }
        private List<Client> Clients { get; set; } = new List<Client>();

        public SimpleServer(string ipAddress, int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public void Start()
        {
            tcpListener.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                Socket socket = tcpListener.AcceptSocket();
                Console.WriteLine("Connection Made");

                Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                t.Start(new Client(socket));
            }
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private void ClientMethod(object obj)
        {
            Client c = (Client)obj;

            Clients.Add(c);

            try
            {
                string receivedMessage = "";

                c.writer.WriteLine("Welcome");
                c.writer.Flush();

                while ((receivedMessage = Console.ReadLine()) != null && receivedMessage.ToLower() != "exit")
                {
                    string msg = GetReturnMessage(receivedMessage);

                    c.writer.WriteLine(msg);
                    c.writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Clients.Remove(c);
                c.socket.Close();
            }

            
        }

        private string GetReturnMessage(string code)
        {
            return code;
        }
    }
}
