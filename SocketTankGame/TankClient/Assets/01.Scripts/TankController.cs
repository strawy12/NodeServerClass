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

        _tankMove.Init(this);
        _turretController.Init(this);
    }

    private void Update()
    {
        if (isRemote == false)
        {
            _tankMove.CheckInput();
            _turretController.CheckInput();
        }
    }

    public void SetUp(bool isPlayer, int playerId)
    {
        isRemote = !isPlayer;
        isEnemy = !isPlayer;

        id = playerId;

        if (isRemote == false)
        {
            StartCoroutine(SendPositionAndRotation());
        }
    }

    private IEnumerator SendPositionAndRotation()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.04f); // 초당 25번

            Vector3 pos = transform.position;
            float rot = _tankBody.rotation.eulerAngles.z;
            float turretRot = _tankTurret.rotation.eulerAngles.z;

            Position info = new Position { X = pos.x, Y = pos.y, Rotate = rot, TurretRotate = turretRot };

            C_Move cMove = new C_Move { PlayerID = id, Position = info };

            NetworkManager.Instance.RegisterSend((ushort)MSGID.CMove, cMove);

        }
    }

    public void OnFire(Vector2 pos, Vector2 dir, bool isEnemy, int fireId)
    {
        _turretController.Fire(pos, dir, isEnemy, fireId);
    }

    public void SetPositionAndRotation(PosAndRot data)
    {
        _tankMove.SetPositionAndRotation(data.pos, data.rot);

        if (isRemote)
        {
            Debug.Log(11);
        }
        _turretController.SetTurretRotation(data.turretRot);
    }

    public void SetDamage(int damage)
    {
        Debug.Log($"{damage} 를 입었습니다.");
    }

    public void DestroyTank()
    {
        Debug.Log("탱크가 파괴됩니다");
    }
}
