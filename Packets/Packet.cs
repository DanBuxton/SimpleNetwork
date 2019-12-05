using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Packets
{
    public enum PacketType
    {
        EMPTY,
        NICKNAME,
        DIRECTMESSAGE,
        CHATMESSAGE,
        CLIENTLIST,
        //ENDPOINT
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
    public class ClientListPacket : Packet
    {
        public List<string> Names { get; private set; }

        public ClientListPacket(params string[] names)
        {
            Type = PacketType.CLIENTLIST;
            Names = names.ToList();
        }
        public ClientListPacket(List<string> names)
        {
            Type = PacketType.CLIENTLIST;
            Names = names;
        }
    }

    [Serializable]
    public class DirectMessagePacket : ChatMessagePacket
    {
        public string To { get; set; } = string.Empty;

        public DirectMessagePacket(string msg, string to) : base(msg)
        {
            To = to;
            Type = PacketType.DIRECTMESSAGE;
        }
    }

    //[Serializable]
    //public class LoginPacket : Packet
    //{
    //    public EndPoint EndPoint { get; set; }

    //    public LoginPacket(EndPoint endPoint)
    //    {
    //        Type = PacketType.ENDPOINT;
    //        EndPoint = endPoint;
    //    }
    //}
}
