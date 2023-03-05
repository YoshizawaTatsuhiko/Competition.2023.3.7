using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private Vector3 _playerPos = Vector3.zero;

    private void Update()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (_playerPos == null)
        {
            _playerPos = Vector3.zero;
        }

        float x = Mathf.Round(_playerPos.x);
        float z = Mathf.Round(_playerPos.z);

        transform.position = new Vector3(x, _playerPos.y + 5f, z);
    }
}
