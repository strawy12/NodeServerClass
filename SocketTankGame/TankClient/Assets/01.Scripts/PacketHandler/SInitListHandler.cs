using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInitListHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_InitList list = packet as S_InitList;

        TankController player = GameManager.Instance.Player;
        foreach (PlayerInfo user in list.Players)
        {
            if (user.PlayerId == player.id) continue;

            PosAndRot posData = Util.ChangePositionInfo(user.Position);

            TankController tank = GameManager.Instance.SpawnTank(posData.pos, user.PlayerId, false);
            tank.PlayerName = user.Name;
        }
    }
}
