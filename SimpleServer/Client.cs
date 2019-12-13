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
        private NetworkStream TCPStream { get; set; }
        public BinaryReader TCPReader { get; private set; }
        public BinaryWriter TCPWriter { get; private set; }

        private Socket UDPSocket { get; set; }

        public MemoryStream Ms { get; set; } = new MemoryStream();
        public BinaryFormatter Bf { get; private set; } = new BinaryFormatter();

        public string Nickname { get; set; } = string.Empty;

        public Client(Socket socket)
        {
            Console.Title = "Server";

            TCPSocket = socket;
            UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            TCPStream = new NetworkStream(socket, true);

            TCPReader = new BinaryReader(TCPStream, Encoding.UTF8);
            TCPWriter = new BinaryWriter(TCPStream, Encoding.UTF8);
        }

        public void UDPConnect(EndPoint clientConnection)
        {
            UDPSocket.Connect(clientConnection);
        }

        public void UDPSend(Packet packet)
        {

        }

        public void Close()
        {
            TCPSocket.Close();
        }
    }
}
