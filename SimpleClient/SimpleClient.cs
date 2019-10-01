using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    class SimpleClient
    {
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        public SimpleClient()
        {

        }

        public bool Connect(string ipAddress, int port)
        {
            bool result = true;

            try
            {
                tcpClient.ConnectAsync(ipAddress, port);

                stream = tcpClient.GetStream();

                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                tcpClient.Close();

                result = false;
            }

            return result;
        }

        public void Run()
        {
            string userInput;
            ProcessServerResponse();

            while ((userInput = Console.ReadLine()) != null)
            {
                writer.WriteLine(userInput);

                writer.Flush();
            }
        }

        public void ProcessServerResponse()
        {
            Console.WriteLine($"Server says: {reader.ReadLine()}\n");
        }
    }
}
