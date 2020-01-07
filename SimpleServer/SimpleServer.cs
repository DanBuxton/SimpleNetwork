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
using System.Drawing;

namespace SimpleServer
{
    class SimpleServer
    {
        private TcpListener TCPListener { get; set; }

        private List<Client> Clients { get; set; } = new List<Client>();
        protected List<string> Nicknames { get; set; } = new List<string>();

        public Bitmap CurrentImage { get; set; } = null;
        public Point CurrentLocation { get; set; } = Point.Empty;

        public SimpleServer(string ipAddress, int port)
        {
            TCPListener = new TcpListener(IPAddress.Parse(ipAddress), port);

            Start(ipAddress, port);
        }

        public void Start(string ip, int port)
        {
            TCPListener.Start();
            Console.WriteLine("Listening on " + ip + " for port " + port + Environment.NewLine);

            while (true)
            {
                Socket socket = TCPListener.AcceptSocket();

                Thread t = new Thread(new ParameterizedThreadStart(TCPClientMethod));
                t.Start(new Client(socket));
            }
        }

        public void Stop()
        {
            TCPListener.Stop();
        }

        public void TCPSend(Packet data, Client to = null)
        {
            if (to == null)
                Clients.ForEach((c) =>
                {
                    c.TCPSend(data);

                    //c.Ms = new MemoryStream();
                    //c.Bf.Serialize(c.Ms, data);
                    //byte[] buffer = c.Ms.GetBuffer();

                    //c.TCPWriter.Write(buffer.Length);
                    //c.TCPWriter.Write(buffer);
                    //c.TCPWriter.Flush();
                });
            else
            {
                to.TCPSend(data);

                //to.Ms = new MemoryStream();
                //to.Bf.Serialize(to.Ms, data);
                //byte[] buffer = to.Ms.GetBuffer();

                //to.TCPWriter.Write(buffer.Length);
                //to.TCPWriter.Write(buffer);
                //to.TCPWriter.Flush();
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

            Console.WriteLine("TCP Connection Made");

            try
            {

                TCPSend(new ChatMessagePacket("Welcome"), c);

                if (CurrentImage != null)
                {
                    TCPSend(new ImagePacket(CurrentImage), c);
                    //TCPSend(new ImagePositionPacket(0, 0), c);
                }

                //TCPSendToEveryoneBut(new ChatMessagePacket(string.Format("{0} Connected", GetNickname(c))), c);

                int noOfIncomingBytes;
                while ((noOfIncomingBytes = c.TCPReader.ReadInt32()) != 0)
                {
                    ProcessResponce(data: c.Bf.Deserialize(new MemoryStream(c.TCPReader.ReadBytes(noOfIncomingBytes))) as Packet, c: c);
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

                c.TCPClose();
            }
        }
        //private void UDPClientMethod(object obj)
        //{
        //    Client c = (Client)obj;

        //    Clients.Add(c);

        //    try
        //    {
        //        Console.WriteLine("UDP Connection Made");

        //        while (c.TCPConnected)
        //        {
        //            UDPProcessResponce(c, c.UDPRead());
        //        }
        //    }
        //    catch (Exception) { }
        //    finally
        //    {
        //        Console.WriteLine("UDP Disconnected");
        //        UDPSendClientList();
        //    }

        //    c.UDPClose();
        //}

        private void ProcessResponce(Client c, Packet data)
        {
            string msg;

            switch (data.Type)
            {
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

                    Client to = null;
                    for (int i = 0; i < Clients.Count && to == null; i++)
                    {
                        //Client c1 = Clients[i];
                        if (GetNickname(Clients[i]).Equals(pack.To)) to = Clients[i];
                    }
                    Console.WriteLine("Private message: {0} -> {1}", GetNickname(c), GetNickname(to));

                    TCPSend(data, to);
                    break;
                case PacketType.CHATMESSAGE:
                    var packet = data as ChatMessagePacket;

                    for (int i = 0; i < Clients.Count; i++)
                    {
                        msg = (GetNickname(c).Equals(GetNickname(Clients[i])) ? "You" : GetNickname(c)) + ": " + packet.Message;
                        TCPSend(new ChatMessagePacket(msg), Clients[i]);
                    }
                    msg = GetNickname(c) + ": " + packet.Message;

                    Console.WriteLine(msg);
                    break;
                case PacketType.IMAGE:
                    CurrentImage = (data as ImagePacket).Image;
                    Console.WriteLine("Image ({0}) received from {1}", (data as ImagePacket).FileName, GetNickname(c));
                    TCPSend(data);
                    break;
                case PacketType.IMAGEPOS:
                    Console.WriteLine("In image pos");
                    Console.WriteLine("x:{0}; y:{1}", (data as ImagePositionUpdatePacket).X, (data as ImagePositionUpdatePacket).Y);

                    //Console.WriteLine("New image position received: x: {0}; y: {1);", (data as ImagePositionUpdatePacket).X, (data as ImagePositionUpdatePacket).Y);
                    if (!(data as ImagePositionUpdatePacket).IsLogOnly)
                        TCPSend(data);

                    break;
                case PacketType.LOGIN:
                case PacketType.EMPTY:
                case PacketType.CLIENTLIST:
                default:
                    break;
            }
        }
        //private void UDPProcessResponce(Client c, Packet data)
        //{
        //    string msg;

        //    switch (data.Type)
        //    {
        //        case PacketType.NICKNAME:
        //            c.Nickname = (data as NicknamePacket).Name;
        //            Nicknames.Add(c.Nickname);
        //            Console.WriteLine("Nickname set: {0}", c.Nickname);
        //            UDPSendClientList();
        //            break;
        //        case PacketType.DIRECTMESSAGE:
        //            // To specific person
        //            var pack = data as DirectMessagePacket;
        //            pack.From = GetNickname(c);
        //            Client client = null;
        //            foreach (Client c1 in Clients)
        //            {
        //                if (c1.Nickname.Equals(pack.To)) { client = c1; break; }
        //            }
        //            Console.WriteLine("Private message: {0} -> {1}", GetNickname(c), GetNickname(client));

        //            TCPSend(data, client);
        //            break;
        //        case PacketType.CHATMESSAGE:
        //            msg = GetNickname(c) + ": " + (data as ChatMessagePacket).Message;
        //            Console.WriteLine(msg);

        //            TCPSend(new ChatMessagePacket(msg));
        //            break;
        //        //case PacketType.LOGIN:
        //        //    c.UDPConnect((data as LoginPacket).EndPoint);

        //        //    Thread t = new Thread(new ParameterizedThreadStart(UDPClientMethod));
        //        //    t.Start(c);
        //        //    break;
        //        case PacketType.EMPTY:
        //        case PacketType.CLIENTLIST:
        //        default:
        //            break;
        //    }
        //}

        private void TCPSendClientList()
        {
            foreach (Client cl in Clients)
            {
                var namesNew = new List<string>(Nicknames);
                if (namesNew.Remove(cl.Nickname))
                {
                    cl.TCPSend(new ClientListPacket(namesNew));
                    Console.WriteLine("Client List sent to: " + cl.Nickname);
                }
            }
        }
        //private void UDPSendClientList()
        //{
        //    if (Nicknames.Count > 1)
        //        foreach (Client cl in Clients)
        //        {
        //            var namesNew = new List<string>(Nicknames);
        //            if (namesNew.Remove(cl.Nickname))
        //            {
        //                //cl.UDPSend(new ClientListPacket(namesNew));
        //                Console.WriteLine("Client List sent to: " + cl.Nickname);
        //            }
        //        }
        //}

        private string GetNickname(Client c)
        {
            return c.Nickname != string.Empty ? c.Nickname : c.GetHashCode().ToString();
        }
    }
}
