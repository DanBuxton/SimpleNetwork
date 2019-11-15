using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Packets
{
    public enum PacketType
    {
        EMPTY,
        NICKNAME,
        CHATMESSAGE,
        CLIENTLIST,
        ENDPOINT
    }

    [Serializable]
    public class Packet
    {
        public PacketType Type { get; set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string Message { get; set; }

        public ChatMessagePacket(string msg)
        {
            Type = PacketType.CHATMESSAGE;
            Message = msg;
        }
    }

    [Serializable]
    public class NicknamePacket : Packet
    {
        public string Name { get; set; }

        public NicknamePacket(string name)
        {
            Type = PacketType.NICKNAME;
            Name = name;
        }
    }

    [Serializable]
    class ClientListPacket : Packet
    {


        public ClientListPacket()
        {
            Type = PacketType.CLIENTLIST;
        }
    }

    [Serializable]
    class LoginPacket : Packet
    {
        public EndPoint EndPoint { get; set; }

        public LoginPacket(EndPoint endPoint)
        {
            Type = PacketType.ENDPOINT;
            EndPoint = endPoint;
        }
    }
}
