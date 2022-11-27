using GGM.Proto.Tank;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager
{
    public static TankManager Instance = null;

    private Dictionary<int, TankController> _remoteTankList;

    public TankManager()
    {
        _remoteTankList = new Dictionary<int, TankController>();
    }

    public void AddRemoteTank(TankController tank)
    {
        _remoteTankList.Add(tank.id, tank);
    }

    public void RemoveRemoteTank(int id)
    {
        TankController tank = null;

        if(_remoteTankList.TryGetValue(id, out tank))
        {
            _remoteTankList.Remove(id);

            Debug.Log($"Player {tank.PlayerName} - {tank.id} 님이 접속 종료");
            GameObject.Destroy(tank.gameObject);
        }
    }
    public TankController GetRemotetank(int id)
    {
        TankController tank = null;
        _remoteTankList.TryGetValue(id, out tank);

        return tank;
    }

    public void UpdateRemoteTank(RepeatedField<PlayerInfo> players)
    {
        foreach (PlayerInfo p in players)
        {
            PosAndRot data = Util.ChangePositionInfo(p.Position);

            TankController tank = null;

            if(_remoteTankList.TryGetValue(p.PlayerId, out tank))
            {
                tank.SetPositionAndRotation(data);
            }
        }
    }
}
