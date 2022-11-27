using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SInitHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Init sInit = packet as S_Init;

        Vector3 pos = MapManager.Instance.GetWorldPos(new Vector3Int((int)sInit.SpawnPosition.X, (int)sInit.SpawnPosition.Y, 0));

        TankController tank = GameManager.Instance.SpawnTank(pos, sInit.PlayerId, true);

        NetworkManager.Instance.SessionId = sInit.PlayerId;

        Vector3 spawnedPos = tank.transform.position;
        Position info = new Position { Rotate = 0, X = spawnedPos.x, Y = spawnedPos.y };

        C_Enter cEnter = new C_Enter {Name = "Gondr", Position = info };
        tank.PlayerName = "Gondr";

        NetworkManager.Instance.RegisterSend((ushort)MSGID.CEnter, cEnter);

    }
}
