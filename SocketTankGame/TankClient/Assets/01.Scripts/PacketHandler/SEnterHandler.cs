using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEnterHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Enter enterMsg = packet as S_Enter;

        PlayerInfo player = enterMsg.Player;

        PosAndRot data = Util.ChangePositionInfo(player.Position);
        TankController remoteTank = GameManager.Instance.SpawnTank(data.pos,player.PlayerId,false);

        remoteTank.PlayerName = player.Name;

    }
}
