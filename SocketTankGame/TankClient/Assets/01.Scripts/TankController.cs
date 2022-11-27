using GGM.Proto.Tank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : NetworkObject
{
    public bool isRemote = false;
    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;

    private TankMove _tankMove;

    [SerializeField]
    private Transform _tankBody;
    public Transform TankBody => _tankBody;

    private TurretController _turretController;

    [SerializeField]
    private Transform _tankTurret;
    public Transform TankTurret => _tankTurret;

    public string PlayerName;
    public bool isEnemy;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _tankMove = GetComponent<TankMove>();

        _turretController = GetComponent<TurretController>();

    }

    private void Start()
    {
        _tankMove.Init(this);
    }  

    private void Update()
    {
        if(isRemote == false)
        {
            _tankMove.CheckInput();
        }
    }

    public void SetUp(bool isPlayer, int playerId)
    {
        isRemote = !isPlayer;
        isEnemy = !isPlayer;

        id = playerId;

        if(isRemote == false)
        {
            StartCoroutine(SendPositionAndRotation());
        }
    }

    private IEnumerator SendPositionAndRotation()
    {
        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.04f); // ÃÊ´ç 25¹ø

            Vector3 pos = transform.position;
            float rot = _tankBody.rotation.eulerAngles.z;
            float turretRot = _tankTurret.rotation.eulerAngles.z;

            Position info = new Position { X = pos.x, Y = pos.y, Rotate = rot, TurretRotate = turretRot };

            C_Move cMove = new C_Move { PlayerID = id, Position = info };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CMove, cMove);

        }    
    }

    public void SetPositionAndRotation(PosAndRot data)
    {
        _tankMove.SetPositionAndRotation(data.pos, data.rot);
    }
}
