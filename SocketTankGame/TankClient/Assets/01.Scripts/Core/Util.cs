using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PosAndRot
{
    public Vector3 pos;
    public Quaternion rot;
    public Quaternion turretRot;
}

public class Util 
{
    public static PosAndRot ChangePositionInfo(Position info)
    {
        PosAndRot data = new PosAndRot
        {
            pos = new Vector3(info.X, info.Y, 0f),
            rot = Quaternion.Euler(0, 0, info.Rotate),
            turretRot = Quaternion.Euler(0, 0, info.TurretRotate)
        };

        return data;
    }
}
