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

            //Console.WriteLine("Listening...");

            while (true)
            {
                Socket socket = TCPListener.AcceptSocket();

                Thread t = new Thread(new ParameterizedThreadStart(TCPClientMethod));
                t.Start(new Client(socket));
            }
        }

        public void TCPStop()
        {
            TCPListener.Stop();
        }

        public void TCPSend(Packet data, Client to = null)
        {
            if (to == null)
                Clients.ForEach((c) =>
                {
                    c.Ms = new MemoryStream();
                    c.Bf.Serialize(c.Ms, data);
                    byte[] buffer = c.Ms.GetBuffer();

                    c.TCPWriter.Write(buffer.Length);
                    c.TCPWriter.Write(buffer);
                    c.TCPWriter.Flush();
                });
            else
            {
                to.Ms = new MemoryStream();
                to.Bf.Serialize(to.Ms, data);
                byte[] buffer = to.Ms.GetBuffer();

                to.TCPWriter.Write(buffer.Length);
                to.TCPWriter.Write(buffer);
                to.TCPWriter.Flush();
            }
        }

        private void TCPSendToEveryoneBut(Packet data, Client c)
        {
            foreach (var cl in Clients)
            {
                if (c == cl) continue;
                TCPSend(data, cl);
            }
        }

        private void TCPClientMethod(object obj)
        {
            Client c = (Client)obj;

            Clients.Add(c);

            Console.WriteLine("Connection Made");

            try
            {
                int noOfIncomingBytes;

                TCPSend(new ChatMessagePacket("Welcome"), c);

                //TCPSendToEveryoneBut(new ChatMessagePacket(string.Format("{0} Connected", GetNickname(c))), c);

                while ((noOfIncomingBytes = c.TCPReader.ReadInt32()) != 0)
                {
                    TCPProcessResponse(c.Bf.Deserialize(new MemoryStream(c.TCPReader.ReadBytes(noOfIncomingBytes))) as Packet, c);
                }
            }
            catch (Exception) { }
            finally
            {
                Clients.Remove(c);

                Console.WriteLine("Client Disconnected");

                TCPSend(new ChatMessagePacket(string.Format("{0} Disconnected", GetNickname(c))));

                Nicknames.Remove(GetNickname(c));
                TCPSendClientList();

                c.Close();
            }
        }

        private void TCPProcessResponse(Packet data, Client c)
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
                    TCPSendClientList();
                    break;
                case PacketType.DIRECTMESSAGE:
                    // To specific person
                    var pack = data as DirectMessagePacket;
                    pack.From = GetNickname(c);
                    Client client = null;
                    foreach (Client c1 in Clients)
                    {
                        if (c1.Nickname.Equals(pack.To)) { client = c1; break; }
                    }
                    Console.WriteLine("Private message: {0} -> {1}", GetNickname(c), GetNickname(client));

                    TCPSend(data, client);
                    break;
                case PacketType.CHATMESSAGE:
                    msg = GetNickname(c) + ": " + (data as ChatMessagePacket).Message;
                    Console.WriteLine(msg);

                    TCPSend(new ChatMessagePacket(msg));
                    break;
                case PacketType.CLIENTLIST:
                default:
                    break;
            }
        }

        private void UDPSendClientList()
        {
            if (Nicknames.Count > 1)
                foreach (Client cl in Clients)
                {
                    var namesNew = new List<string>(Nicknames);
                    if (namesNew.Remove(cl.Nickname))
                    {
                        TCPSend(new ClientListPacket(namesNew), cl);
                        Console.WriteLine("Client List sent to: " + cl.Nickname);
                    }
                }
        }

        private string GetNickname(Client c)
        {
            return c.Nickname != string.Empty ? c.Nickname : c.GetHashCode().ToString();
        }
    }
}
