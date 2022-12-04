using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFireHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Fire fire = packet as S_Fire;

        Vector2 pos = new Vector2(fire.X, fire.Y);
        Vector2 dir = new Vector2(fire.DirX, fire.DirY);
        int fireId = fire.FireId;

        if(fire.PlayerId == NetworkManager.Instance.SessionId)
        {
            GameManager.Instance.Player.OnFire(pos, dir, false, fireId);
        }

        else
        {
            TankController remoteTank = TankManager.Instance.GetRemotetank(fire.PlayerId);
            remoteTank.OnFire(pos, dir, true, fireId);
        }
    }

}
