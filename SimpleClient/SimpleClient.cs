using Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private MemoryStream ms = new MemoryStream();
        private readonly BinaryFormatter bf = new BinaryFormatter();
        private Thread readerThread;
        private readonly ClientForm messageForm;

        public SimpleClient()
        {
            messageForm = new ClientForm(this);

            Application.Run(messageForm);
        }

        public bool Connect(string hostname, int port)
        {
            bool result = true;

            try
            {
                tcpClient.Connect(hostname, port);

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
            ms = new MemoryStream();
            bf.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            writer.Write(buffer.Length);
            writer.Write(buffer);
            writer.Flush();
        }

        public void SendMessage(string message) => Send(new ChatMessagePacket(message));
        public void SendDirectMessage(string to, string msg) => Send(new DirectMessagePacket(msg, to));
        public void SendNickname(string name) => Send(new NicknamePacket(name));

        public void Stop()
        {
            try
            {
                readerThread.Abort();
            }
            catch (Exception) { }

            tcpClient.Close();
        }

        public void ProcessServerResponse()
        {
            while (true)
            {
                string msg = string.Empty;

                ms = new MemoryStream(reader.ReadBytes(reader.ReadInt32()));
                Packet p = bf.Deserialize(ms) as Packet;
                switch (p.Type)
                {
                    case PacketType.EMPTY:
                        break;
                    case PacketType.NICKNAME:
                        break;
                    case PacketType.DIRECTMESSAGE:
                        var pack = p as DirectMessagePacket;
                        msg = pack.Message;
                        msg = msg.Insert(0, "Private Message From: " + pack.From + "\n\t");
                        break;
                    case PacketType.CHATMESSAGE:
                        msg = ((ChatMessagePacket)p).Message;
                        break;
                    case PacketType.CLIENTLIST:
                        messageForm.UpdateClientList((p as ClientListPacket).Names);
                        break;
                    default:
                        break;
                }

                if (msg != string.Empty)
                    messageForm.UpdateChatWindow($"{msg}\n");
            }
        }
    }
}
