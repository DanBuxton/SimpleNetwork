using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    class Client
    {
        private Socket Socket { get; set; }
        private NetworkStream Stream { get; set; }
        public StreamReader Reader { get; private set; }
        public StreamWriter Writer { get; private set; }

        public Client(Socket socket)
        {
            Console.Title = "Server";

            this.Socket = socket;

            Stream = new NetworkStream(socket, true);

            Reader = new StreamReader(Stream, Encoding.UTF8);
            Writer = new StreamWriter(Stream, Encoding.UTF8);
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
