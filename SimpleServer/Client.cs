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
        private Socket socket { get; set; }
        private NetworkStream stream { get; set; }
        public StreamReader reader { get; private set; }
        public StreamWriter writer { get; private set; }

        public Client(Socket socket)
        {
            Console.Title = "Server";

            this.socket = socket;

            stream = new NetworkStream(socket, true);

            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8);
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
