using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public bool IsRemote = false;
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
        C_Enter cEnter = new C_Enter();
        cEnter.Name = PlayerName;

        Vector3 point = MapManager.Instance.GetTilePos(transform.position);
        cEnter.Position = new Position { Rotate = transform.eulerAngles.z, X = point.x, Y = point.y, TurretRotate = _tankTurret.eulerAngles.z };

        NetworkManager.Instance.RegisterSend((ushort)MSGID.CEnter, cEnter);

        _tankMove.Init(this);
    }

    private void Update()
    {
        if(IsRemote == false)
        {
            _tankMove.CheckInput();
        }
    }
}
