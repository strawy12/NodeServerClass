using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    [SerializeField]
    private TankMoveSO _tankMoveSO;

    private TankController _controller;

    private Vector3 _targetPos;
    private Quaternion _targetRot;

    public void Init(TankController ctrl)
    {
        _controller = ctrl;
    }

    public void CheckInput()
    {
        CheckMove();
        CheckRotate();
    }

    private void CheckRotate()
    {
        float angle = 0;
        if (Input.GetKey(KeyCode.A))
        {
            angle = _tankMoveSO.RotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            angle = -_tankMoveSO.RotateSpeed * Time.deltaTime;
        }

        _controller.TankBody.rotation *= Quaternion.Euler(0, 0, angle);
    }

    private void CheckMove()
    {
        Vector2 velocity = _controller.Rigidbody.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += (Vector2)_controller.TankBody.up * _tankMoveSO.Accelration * Time.deltaTime;
        }


        else if (Input.GetKey(KeyCode.S))
        {
            velocity += (Vector2)_controller.TankBody.up * _tankMoveSO.Accelration * Time.deltaTime * -1f;
        }

        else
        {
            velocity -= velocity * _tankMoveSO.DeAccelration * Time.deltaTime;
        }

        velocity = Vector2.ClampMagnitude(velocity, _tankMoveSO.MaxSpeed);
        _controller.Rigidbody.velocity = velocity;
    }

    public void StopImmedatelly()
    {
        _controller.Rigidbody.velocity = Vector2.zero;
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        _targetPos = pos;
        _targetRot = rot;
    }

    private void Update()
    {
        if (_controller.isRemote)
        {
            Vector3 pos = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * 15);
            Quaternion rot = Quaternion.Lerp(_controller.TankBody.rotation, _targetRot, Time.deltaTime * 15);

            transform.position = pos;
            _controller.TankBody.rotation = rot;
        }
    }
}
