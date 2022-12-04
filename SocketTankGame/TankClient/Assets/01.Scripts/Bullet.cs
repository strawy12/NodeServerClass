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

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 dir, float power, int damage)
    {
        _rigid.velocity = dir * power;
        _damage = damage;

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public override void Reset()
    {
        _rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
