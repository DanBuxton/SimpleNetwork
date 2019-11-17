﻿using System;
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
        DIRECTMESSAGE,
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
    public class ClientListPacket : Packet
    {
        public List<string> Names { get; set; }

        public ClientListPacket(params string[] names)
        {
            Type = PacketType.CLIENTLIST;
            Names = names.ToList();
        }
    }

    [Serializable]
    public class DirectMessagePacket : ChatMessagePacket
    {
        public DirectMessagePacket(string msg) : base(msg)
        {
            Type = PacketType.DIRECTMESSAGE;
        }
    }

    [Serializable]
    public class LoginPacket : Packet
    {
        public EndPoint EndPoint { get; set; }

        public LoginPacket(EndPoint endPoint)
        {
            Type = PacketType.ENDPOINT;
            EndPoint = endPoint;
        }
    }
}
