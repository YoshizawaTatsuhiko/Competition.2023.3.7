using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField,Tooltip("ˆÚ“®‘¬“x")] float _speed = 1.0f;
    CharacterController _character = default;

    void Start()
    {
        _character = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        _character.SimpleMove(dir.normalized * _speed);
        //transform.forward = Camera.main.transform.forward;
        Vector3 camDir = Camera.main.transform.forward;
        camDir.y = 0;
        transform.forward = camDir;
    }
}
