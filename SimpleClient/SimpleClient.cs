using Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream tcpStream;
        private BinaryWriter tpcWriter;
        private BinaryReader tcpReader;
        private Thread tcpReaderThread;

        //public UdpClient udpClient;
        //private Thread udpThread;

        private MemoryStream ms = new MemoryStream();
        private readonly BinaryFormatter bf = new BinaryFormatter();
        private readonly ClientForm messageForm;

        public SimpleClient()
        {
            messageForm = new ClientForm(this);

            //t.SetApartmentState(ApartmentState.STA);
            Application.Run(messageForm);
        }

        public bool Connect(string hostname, int port)
        {
            bool result = true;

            tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect(hostname, port);
                tcpStream = tcpClient.GetStream();
                tpcWriter = new BinaryWriter(tcpStream, Encoding.UTF8);
                tcpReader = new BinaryReader(tcpStream, Encoding.UTF8);

                tcpReaderThread = new Thread(new ThreadStart(ProcessServerResponse));

                //udpClient = new UdpClient();
                //udpClient.Connect("127.0.0.1", 4444);
                //TCPSend(new LoginPacket(udpClient.Client.LocalEndPoint));
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
        //public void UDPSend(Packet data)
        //{
        //    ms = new MemoryStream();
        //    bf.Serialize(ms, data);
        //    byte[] buffer = ms.GetBuffer();

        //    udpClient.Send(buffer, buffer.Length);
        //}

        public void TCPSendMessage(string message) => TCPSend(new ChatMessagePacket(message));
        public void TCPSendDirectMessage(string to, string msg) => TCPSend(new DirectMessagePacket(msg, to));
        public void TCPSendNickname(string name) => TCPSend(new NicknamePacket(name));
        public void TCPSendImage(Bitmap img, string fileName) => TCPSend(new ImagePacket(img, fileName));
        //public void TCPSendImagePosition(int x, int y) => TCPSend(new ImagePositionPacket(x, y));
        public void TCPSendImagePositionUpdate(int x, int y) => TCPSend(new ImagePositionUpdatePacket(x, y));
        public void TCPSendImagePositionUpdateLog(int x, int y) => TCPSend(new ImagePositionUpdatePacket(x, y, true));
        //public void UDPSendImagePositionUpdate(int x, int y) => UDPSend(new NewImagePositionPacket(x, y));

        public void Stop()
        {
            try
            {
                //udpThread.Abort();
                tcpReaderThread.Abort();
            }
            catch (Exception) { }

            //udpClient.Close();
            if (tcpClient.Connected)
                tcpClient.Close();
        }

        public void ProcessServerResponse()
        {
            while (true)
            {
                string msg = string.Empty;

                try
                {
                    ms = new MemoryStream(tcpReader.ReadBytes(tcpReader.ReadInt32()));
                    Packet p = bf.Deserialize(ms) as Packet;
                    switch (p.Type)
                    {
                        case PacketType.DIRECTMESSAGE:
                            var pack = p as DirectMessagePacket;
                            msg = pack.Message;
                            msg = msg.Insert(0, "(Private) " + pack.From + ": ");
                            break;
                        case PacketType.CHATMESSAGE:
                            msg = ((ChatMessagePacket)p).Message;
                            break;
                        case PacketType.CLIENTLIST:
                            messageForm.UpdateClientList((p as ClientListPacket).Names);
                            break;
                        case PacketType.IMAGE:
                            messageForm.UpdateImage((p as ImagePacket).Image);
                            break;
                        case PacketType.IMAGEPOS:
                            messageForm.UpdateImageLocation((p as ImagePositionUpdatePacket).X, (p as ImagePositionUpdatePacket).Y);
                            break;
                        //case PacketType.LOGIN:
                        //    udpClient.Connect((p as LoginPacket).EndPoint as IPEndPoint);
                        //    udpThread = new Thread(new ThreadStart(UDPProcessServerResponce));
                        //    udpThread.Start();
                        //break;
                        case PacketType.EMPTY:
                        case PacketType.NICKNAME:
                        default:
                            break;
                    }

                    if (msg != string.Empty)
                        messageForm.UpdateChatWindow($"{msg}\n");
                }
                catch (IOException)
                {
                    messageForm.UpdateChatWindow($"Error Connecting - Please reconnect\n");
                    messageForm.UpdateWindow(true);
                    Stop();
                }
            }
        }
        //public void UDPProcessServerResponce()
        //{
        //    while (true)
        //    {
        //        //ms = new MemoryStream(udpClient.R.ReadBytes(udpReader.ReadInt32()));
        //        Packet p = UDPRead();
        //        switch (p.Type)
        //        {
        //            case PacketType.CLIENTLIST:
        //                messageForm.UpdateClientList((p as ClientListPacket).Names);
        //                break;
        //            case PacketType.IMAGE:

        //                break;
        //            case PacketType.EMPTY:
        //            case PacketType.NICKNAME:
        //            case PacketType.DIRECTMESSAGE:
        //            case PacketType.CHATMESSAGE:
        //            default:
        //                break;
        //        }
        //    }
        //}

        private Packet DeserializePacket(byte[] buffer)
        {
            return bf.Deserialize(new MemoryStream(buffer)) as Packet;
        }
        //public Packet UDPRead()
        //{
        //    IPEndPoint ipEP = udpClient.Client.RemoteEndPoint as IPEndPoint;

        //    try
        //    {
        //        return DeserializePacket(udpClient.Receive(ref ipEP));
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
