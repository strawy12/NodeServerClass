using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    private int _hp;
    private int _maxHealth;
    public int HP => _hp;
    private TankController _controller;

    public void Init(TankController controller, int maxHealth)
    {
        _hp = _maxHealth = maxHealth;
        _controller = controller;
    }

    public void AddValue(int value)
    {
        _hp = Mathf.Clamp(value + _hp, 0, _maxHealth);

        if(_hp == 0)
        {
            _controller.DestroyTank();
        }
    }

}
