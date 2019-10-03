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

        public bool Connect(string ipAddress, int port)
        {
            bool result = true;

            try
            {
                tcpClient.Connect(ipAddress, port);

                stream = tcpClient.GetStream();

                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                result = false;
            }

            return result;
        }

        public void Run()
        {
            string userInput;

            ProcessServerResponse();

            while ((userInput = Console.ReadLine()) != null && userInput.ToLower() != "exit")
            {
                writer.WriteLine(userInput);
                writer.Flush();

                ProcessServerResponse();
            }

            tcpClient.Close();
        }

        public void ProcessServerResponse()
        {
            Console.WriteLine($"Server says: {reader.ReadLine()}");
        }
    }
}
