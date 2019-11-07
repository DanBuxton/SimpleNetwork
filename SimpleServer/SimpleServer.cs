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
        private readonly TcpListener tcpListener;
        private List<Client> Clients { get; set; } = new List<Client>();

        protected List<string> Nicknames { get; set; } = new List<string>();

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

            Console.WriteLine("Connection Made");

            try
            {
                string receivedMessage = "";

                c.Writer.WriteLine("Welcome");
                c.Writer.Flush();

                while ((receivedMessage = c.Reader.ReadLine()) != null && receivedMessage.ToLower() != "exit")
                {
                    string msg = GetReturnMessage(receivedMessage);

                    Console.WriteLine(msg);

                    Clients.ForEach((cl) =>
                    {
                        cl.Writer.WriteLine(msg);
                        cl.Writer.Flush();
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Clients.Remove(c);

                Console.WriteLine("Client Disconnected");

                c.Close();
            }
        }

        private string GetReturnMessage(string code)
        {
            return code;
        }
    }
}
