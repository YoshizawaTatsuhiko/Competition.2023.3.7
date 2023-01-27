using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{
    [SerializeField,Tooltip("ˆÚ“®‘¬“x")]
    private float _speed = 1.0f;
    private Rigidbody _rigidbody = null;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _rigidbody.velocity = dir * _speed;
        Vector3 camDir = Camera.main.transform.forward;
        camDir.y = 0;
        transform.forward = camDir;
    }
}
