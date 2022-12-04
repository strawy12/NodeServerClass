using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private TankController _controller;
    private Camera _mainCam;

    [SerializeField]
    private Transform _firePos;

    [SerializeField]
    private float _firePower = 50f;

    [SerializeField]
    private int _bulletPower = 10;

    public void Init(TankController ctrl)
    {
        _controller = ctrl;
        _mainCam = Camera.main;
    }

    public void CheckInput()
    {
        Vector3 worldMousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;
        Vector3 delta = worldMousePos - transform.position;

        float degree = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        _controller.TankTurret.rotation = Quaternion.AngleAxis(degree - 90f, Vector3.forward);

        if(Input.GetMouseButtonDown(0))
        {
            TryToFire();
        }
    }

    private void TryToFire()
    {
        Vector3 firePos = _firePos.position;
        Vector3 direction = _firePos.up;

        C_Fire fire = new C_Fire
        {
            PlayerId = _controller.id,
            X = firePos.x,
            Y = firePos.y,
            Z = firePos.z,
            DirX = direction.x,
            DirY = direction.y
        };

        NetworkManager.Instance.RegisterSend((ushort)MSGID.CFire, fire);
    }

    public void Fire(Vector2 pos, Vector2 dir, bool isEnemy, int fireId)
    {
        Bullet b = PoolManager.Instance.Pop("Bullet") as Bullet;
        b.Id = fireId;
        b.IsEnemy = isEnemy;
        b.SetPosition(pos);
        b.Fire(dir, _firePower, _bulletPower);

        GameManager.Instance.AddActiveBullet(fireId, b);
    }

    public void SetTurretRotation(Quaternion rot)
    {
        _controller.TankTurret.rotation = rot;
    }
}
