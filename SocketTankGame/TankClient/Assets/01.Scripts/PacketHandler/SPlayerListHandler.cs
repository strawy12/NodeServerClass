using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayerListHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_PlayerList listMsg = packet as S_PlayerList;

        TankManager.Instance.UpdateRemoteTank(listMsg.Players);
    }
}
