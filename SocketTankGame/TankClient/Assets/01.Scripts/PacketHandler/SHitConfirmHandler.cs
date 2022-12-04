using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHitConfirmHandler : IPacketHandler
{
    public void Process(IMessage packet)
    {
        S_Hit_Confirm confirm = packet as S_Hit_Confirm;

        if (confirm.IsIgnore) return;

        if(confirm.PlayerId == NetworkManager.Instance.SessionId)
        {
            GameManager.Instance.Player.SetDamage(confirm.Damage);
        }
        else
        {
            TankController remoteTank = TankManager.Instance.GetRemotetank(confirm.PlayerId);
            remoteTank?.SetDamage(confirm.Damage);
        }

        GameManager.Instance.RemoveActiveBullet(confirm.FireId);
    }

}
