using GGM.Proto.Tank;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPacketHandler
{
    public void Process(IMessage packet);
}
public class PacketManager
{
    private Dictionary<ushort, Action<ArraySegment<byte>, ushort>> _OnRecv;
    private Dictionary<ushort, IPacketHandler> _Handlers;

    public PacketManager()
    {
        _OnRecv = new Dictionary<ushort, Action<ArraySegment<byte>, ushort>>();
        _Handlers = new Dictionary<ushort, IPacketHandler>();
        Register();
    }

    private void Register()
    {
       _OnRecv.Add((ushort)MSGID.SInit, MakePacket<S_Init>);
        _Handlers.Add((ushort)MSGID.SInit, new SInitHandler());

        _OnRecv.Add((ushort)MSGID.SEnter, MakePacket<S_Enter>);
        _Handlers.Add((ushort)MSGID.SEnter, new SEnterHandler());

        _OnRecv.Add((ushort)MSGID.SInitlist, MakePacket<S_InitList>);
        _Handlers.Add((ushort)MSGID.SInitlist, new SInitListHandler());

        _OnRecv.Add((ushort)MSGID.SPlayerlist, MakePacket<S_PlayerList>);
        _Handlers.Add((ushort)MSGID.SPlayerlist, new SPlayerListHandler());

        _OnRecv.Add((ushort)MSGID.SFire, MakePacket<S_Fire>);
        _Handlers.Add((ushort)MSGID.SFire, new SFireHandler());
    }

    public IPacketHandler GetPacketHandler(ushort id)
    {
        IPacketHandler handler = null;
        if(_Handlers.TryGetValue(id, out handler))
        {
            return handler;
        }

        else
        {
            return null;
        }
    }

    public int OnRecvPacket(ArraySegment<byte> buffer)
    {
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        ushort code = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);
        
        if(_OnRecv.ContainsKey(code))
        {
            _OnRecv[code].Invoke(buffer, code);
        }
        else
        {
            Debug.LogError($"������������������� ({(MSGID)code}, {size})");
        }

        return size;
    }

    private void MakePacket<T>(ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        PacketQueue.Instance.Push(id, pkt);
    }
}

