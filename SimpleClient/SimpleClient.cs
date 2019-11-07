using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        private StreamWriter writer;
        private StreamReader reader;
        private Thread readerThread;
        private readonly ClientForm messageForm;
        private readonly NicknameForm nicknameForm = new NicknameForm();

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

                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                result = false;
            }

            readerThread = new Thread(new ThreadStart(ProcessServerResponse));

            // Set nickname
            Application.Run(nicknameForm);
            string nickname = nicknameForm.Name;

            Application.Run(messageForm);

            return result;
        }

        public void Run()
        {
            readerThread.Start();
        }

        public void SendMessage(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        public void Stop()
        {
            readerThread.Abort();

            tcpClient.Close();
        }

        public void ProcessServerResponse()
        {
            while (true)
            {

                messageForm.UpdateChatWindow($"{reader.ReadLine()}\n");
            }
        }
    }
}
