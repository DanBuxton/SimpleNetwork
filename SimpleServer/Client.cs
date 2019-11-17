using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Client
    {
        private Socket Socket { get; set; }
        private NetworkStream Stream { get; set; }
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        public MemoryStream Ms { get; set; } = new MemoryStream();
        public BinaryFormatter Bf { get; private set; } = new BinaryFormatter();

        public string Nickname { get; set; } = string.Empty;

        public Client(Socket socket)
        {
            Console.Title = "Server";

            Socket = socket;

            Stream = new NetworkStream(socket, true);

            Reader = new BinaryReader(Stream, Encoding.UTF8);
            Writer = new BinaryWriter(Stream, Encoding.UTF8);
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
