using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private Transform _player = null;
    private Vector3 _playerPos = Vector3.zero;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (!_player) return;

        _playerPos = _player.position;

        float x = Mathf.Round(_playerPos.x);
        float z = Mathf.Round(_playerPos.z);

        transform.position = new Vector3(x, _playerPos.y + 5f, z);
    }
}
