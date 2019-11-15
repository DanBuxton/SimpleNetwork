using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Packets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleServer
{
    class SimpleServer
    {
        private TcpListener TCPListener { get; set; }

        private List<Client> Clients { get; set; } = new List<Client>();
        protected List<string> Nicknames { get; set; } = new List<string>();

        public SimpleServer(string ipAddress, int port)
        {
            TCPListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public void Start()
        {
            TCPListener.Start();

            Console.WriteLine("Listening...");

            while (true)
            {
                Socket socket = TCPListener.AcceptSocket();

                Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                t.Start(new Client(socket));
            }
        }

        public void Stop()
        {
            TCPListener.Stop();
        }

        public void Send(Packet data)
        {

        }

        private void ClientMethod(object obj)
        {
            Client c = (Client)obj;

            Clients.Add(c);

            Console.WriteLine("Connection Made");

            try
            {
                int noOfIncomingBytes;

                c.Bf.Serialize(c.Ms, new ChatMessagePacket("Welcome"));
                byte[] buffer = c.Ms.GetBuffer();

                c.Writer.Write(buffer.Length);
                c.Writer.Write(buffer);
                c.Writer.Flush();

                Send(new ChatMessagePacket("Welcome"));

                while ((noOfIncomingBytes = c.Reader.ReadInt32()) != 0)
                {
                    string msg = (c.Bf.Deserialize(new MemoryStream(c.Reader.ReadBytes(noOfIncomingBytes))) as ChatMessagePacket).Message;

                    Console.WriteLine(msg);

                    Clients.ForEach((cl) =>
                    {
                        Send(new ChatMessagePacket(msg));
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

                Clients.ForEach((cl) =>
                {
                    Send(new ChatMessagePacket("Client Disconnected"));
                });

                c.Close();
            }
        }

        private string ProcessResponse(Packet data)
        {
            return "";
        }
    }
}
