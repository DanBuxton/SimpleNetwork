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
        private TcpClient tcpClient;
        private NetworkStream tcpStream;
        private BinaryWriter tpcWriter;
        private BinaryReader tcpReader;
        private Thread tcpReaderThread;

        public UdpClient udpClient;
        private NetworkStream udpStream;
        private BinaryWriter udpWriter;
        private BinaryReader udpReader;

        private MemoryStream ms = new MemoryStream();
        private readonly BinaryFormatter bf = new BinaryFormatter();
        private readonly ClientForm messageForm;

        public SimpleClient()
        {
            messageForm = new ClientForm(this);

            Application.Run(messageForm);
        }

        public bool TCPConnect(string hostname, int port)
        {
            bool result = true;

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(hostname, port);
                tcpStream = tcpClient.GetStream();
                tpcWriter = new BinaryWriter(tcpStream, Encoding.UTF8);
                tcpReader = new BinaryReader(tcpStream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");

                result = false;
            }

            tcpReaderThread = new Thread(new ThreadStart(TCPProcessServerResponse));

            return result;
        }

        public void Run()
        {
            tcpReaderThread.Start();
        }

        public void TCPSend(Packet data)
        {
            ms = new MemoryStream();
            bf.Serialize(ms, data);
            byte[] buffer = ms.GetBuffer();

            tpcWriter.Write(buffer.Length);
            tpcWriter.Write(buffer);
            tpcWriter.Flush();
        }

        public void TCPSendMessage(string message) => TCPSend(new ChatMessagePacket(message));
        public void TCPSendDirectMessage(string to, string msg) => TCPSend(new DirectMessagePacket(msg, to));
        public void TCPSendNickname(string name) => TCPSend(new NicknamePacket(name));
        public void TCPSendImage(byte[] img) => TCPSend(new ImagePacket(img));
        public void TCPSendImagePosition(int x, int y) => TCPSend(new NicknamePacket(""));

        public void UDPSendImagePositionUpdate(int x, int y) => TCPSend(new NicknamePacket(""));

        public void TCPStop()
        {
            try
            {
                tcpReaderThread.Abort();
            }
            catch (Exception) { }

            tcpClient.Close();
        }

        public void TCPProcessServerResponse()
        {
            while (true)
            {
                string msg = string.Empty;

                ms = new MemoryStream(tcpReader.ReadBytes(tcpReader.ReadInt32()));
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
