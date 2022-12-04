using GGM.Proto.Tank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableMono
{
    public bool IsEnemy = false;
    private Rigidbody2D _rigid;

    public int Id;
    private int _damage;
    public int Damage => _damage;

    private float _timeToLive = 0.8f;
    private float _currentTime = 0f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime >= _timeToLive)
        {
            Die();
        }

    }

    public void Fire(Vector2 dir, float power, int damage)
    {
        _currentTime = 0f;
        _rigid.velocity = dir * power;
        _damage = damage;

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Die()
    {
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        _rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TankController tank = collision.GetComponent<TankController>();

        if(tank != null && tank.isEnemy != IsEnemy)
        {
            Debug.Log("피격");

            if(tank.isRemote == false)
            {
                Vector3 pos = transform.position;
                C_Hit_Report report = new C_Hit_Report
                {
                    PlayerId = tank.id,
                    FireId = Id,
                    X = pos.x,
                    Y = pos.y,
                    Damage = _damage
                };

                NetworkManager.Instance.RegisterSend((ushort)MSGID.CHitReport, report);
            }
        }

        else if(tank == null)
        {
            Debug.Log(collision);
            Debug.Log("벽에 맞음");
            GameManager.Instance.RemoveActiveBullet(this.Id);
        }
    }
}
