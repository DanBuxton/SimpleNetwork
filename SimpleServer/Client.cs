using Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Client
    {
        private Socket TCPSocket { get; set; }
        //private Socket UDPSocket { get; set; }

        private NetworkStream TCPStream { get; set; }
        public BinaryReader TCPReader { get; private set; }
        public BinaryWriter TCPWriter { get; private set; }

        public MemoryStream Ms { get; set; } = new MemoryStream();
        public BinaryFormatter Bf { get; private set; } = new BinaryFormatter();
        public string Nickname { get; set; } = string.Empty;
        public bool TCPConnected
        {
            get
            {
                return TCPSocket.Connected;
            }
        }

        public Client(Socket socket)
        {
            Console.Title = "Server";

            TCPSocket = socket;
            //UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            TCPStream = new NetworkStream(TCPSocket, true);

            TCPReader = new BinaryReader(TCPStream, Encoding.UTF8);
            TCPWriter = new BinaryWriter(TCPStream, Encoding.UTF8);
        }

        //public void UDPConnect(EndPoint clientConnection)
        //{
        //    UDPSocket.Connect(clientConnection);

        //    TCPSend(new LoginPacket(UDPSocket.LocalEndPoint));
        //}

        public void TCPSend(Packet packet)
        {
            Ms = new MemoryStream();
            Bf.Serialize(Ms, packet);
            byte[] buffer = Ms.GetBuffer();

            TCPWriter.Write(buffer.Length);
            TCPWriter.Write(buffer);
            TCPWriter.Flush();
        }
        //public void UDPSend(Packet packet)
        //{
        //    Ms = new MemoryStream();
        //    Bf.Serialize(Ms, packet);

        //    UDPSocket.Send(Ms.GetBuffer());
        //}

        public Packet TCPRead()
        {
            int noOfIncomingBytes;
            if ((noOfIncomingBytes = TCPReader.ReadInt32()) != 0)
                return DeserializePacket(TCPReader.ReadBytes(noOfIncomingBytes));
            return null;
        }
        //public Packet UDPRead()
        //{
        //    int noOfIncomingBytes;
        //    byte[] bytes = new byte[256];
        //    if ((noOfIncomingBytes = UDPSocket.Receive(bytes)) != 0)
        //        return DeserializePacket(bytes);
        //    return null;
        //}

        private Packet DeserializePacket(byte[] buffer)
        {
            return Bf.Deserialize(new MemoryStream(buffer)) as Packet;
        }

        public void TCPClose()
        {
            TCPSocket.Close();
        }
        //public void UDPClose()
        //{
        //    UDPSocket.Close();
        //}
    }
}
