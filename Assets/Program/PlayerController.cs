using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{
    [SerializeField,Tooltip("移動速度")]
    private float _speed = 1.0f;
    private Rigidbody _rigidbody = null;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        // Playerの操作系統の処理
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")) * _speed;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;

        // Playerの始点操作系統の処理
        Vector3 camDir = Camera.main.transform.forward;
        camDir.y = 0;
        transform.forward = camDir;
    }
}
