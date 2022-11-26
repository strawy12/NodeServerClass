using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SInitHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Init sInit = packet as S_Init;

        GameManager.Instance.CreateTank(sInit.PlayerId, sInit.SpawnPosition);
    }
}
