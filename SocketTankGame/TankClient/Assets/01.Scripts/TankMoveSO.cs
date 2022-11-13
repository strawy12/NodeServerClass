using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Tank/MoveSO")]
public class TankMoveSO : ScriptableObject
{
    [Range(1, 10)]
    public float MaxSpeed;
    [Range(0.1f, 100f)]
    public float Accelration, DeAccelration;
    public float RotateSpeed;
}
 