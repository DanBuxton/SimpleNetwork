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

        public void Send(Packet data, Client to = null)
        {
            if (to == null)
                Clients.ForEach((c) =>
                {
                    c.Ms = new MemoryStream();
                    c.Bf.Serialize(c.Ms, data);
                    byte[] buffer = c.Ms.GetBuffer();

                    c.Writer.Write(buffer.Length);
                    c.Writer.Write(buffer);
                    c.Writer.Flush();
                });
            else
            {
                to.Ms = new MemoryStream();
                to.Bf.Serialize(to.Ms, data);
                byte[] buffer = to.Ms.GetBuffer();

                to.Writer.Write(buffer.Length);
                to.Writer.Write(buffer);
                to.Writer.Flush();
            }
        }

        private void ClientMethod(object obj)
        {
            Client c = (Client)obj;

            Clients.Add(c);

            Console.WriteLine("Connection Made");

            try
            {
                int noOfIncomingBytes;

                Send(new ChatMessagePacket("Welcome"), c);

                while ((noOfIncomingBytes = c.Reader.ReadInt32()) != 0)
                {
                    ProcessResponse(c.Bf.Deserialize(new MemoryStream(c.Reader.ReadBytes(noOfIncomingBytes))) as Packet, c);
                }
            }
            catch (Exception) { }
            finally
            {
                Clients.Remove(c);

                Console.WriteLine("Client Disconnected");

                Send(new ChatMessagePacket(string.Format("{0} Disconnected", c.Nickname != string.Empty ? c.Nickname : c.GetHashCode().ToString())));

                c.Close();
            }
        }

        private void ProcessResponse(Packet data, Client c)
        {
            string msg;

            switch (data.Type)
            {
                case PacketType.EMPTY:
                    break;
                case PacketType.NICKNAME:
                    c.Nickname = (data as NicknamePacket).Name;
                    Nicknames.Add(c.Nickname);
                    Console.WriteLine("Nickname set: {0}", c.Nickname);
                    SendClientList();
                    break;
                case PacketType.DIRECTMESSAGE:
                    // To specific person
                    var pack = data as DirectMessagePacket;
                    Client client = null;
                    foreach (Client c1 in Clients)
                    {
                        if (c1.Nickname.Equals(pack.To)) { client = c1; break; }
                    }
                    Console.WriteLine("Private message sent to {1} from {0}", GetNickname(c), GetNickname(client));

                    Send(data, client);
                    break;
                case PacketType.CHATMESSAGE:
                    msg = GetNickname(c) + ": " + (data as ChatMessagePacket).Message;
                    Console.WriteLine(msg);

                    Send(new ChatMessagePacket(msg));
                    break;
                case PacketType.CLIENTLIST:

                    break;
                default:
                    break;
            }
        }

        private void SendClientList()
        {
            foreach (Client cl in Clients)
            {
                var namesNew = new List<string>(Nicknames);
                if (namesNew.Remove(cl.Nickname))
                {
                    Send(new ClientListPacket(namesNew), cl);
                    Console.WriteLine("Client List sent to: " + cl.Nickname);
                }
            }
        }

        public string GetNickname(Client c)
        {
            return c.Nickname != string.Empty ? c.Nickname : c.GetHashCode().ToString();
        }
    }
}
