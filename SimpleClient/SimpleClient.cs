using Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClient
{
    public class SimpleClient
    {
        private readonly TcpClient tcpClient = new TcpClient();
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private readonly MemoryStream ms = new MemoryStream();
        private readonly BinaryFormatter bf = new BinaryFormatter();
        private Thread readerThread;
        private readonly ClientForm messageForm;

        public SimpleClient()
        {
            messageForm = new ClientForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            bool result = true;

            try
            {
                tcpClient.Connect(ipAddress, port);

                stream = tcpClient.GetStream();

                writer = new BinaryWriter(stream, Encoding.UTF8);
                reader = new BinaryReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                result = false;
            }

            readerThread = new Thread(new ThreadStart(ProcessServerResponse));

            return result;
        }

        public void Run()
        {
            readerThread.Start();
        }

        public void Send(Packet data)
        {
            bf.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }

        public void SendMessage(string message) => Send(new ChatMessagePacket(message));

        public void Stop()
        {
            readerThread.Abort();

            tcpClient.Close();
        }

        public void ProcessServerResponse()
        {
            while (true)
            {
                messageForm.UpdateChatWindow($"{reader.Read()}\n");
            }
        }
    }
}
