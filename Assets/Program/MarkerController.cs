using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    private Transform _playerPos = null;

    private void Update()
    {
        Vector3 pos = Vector3.zero;

        if (_playerPos)
        {
            pos = _playerPos.position;
        }
        else
        {
            _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        float x = Mathf.Round(pos.x);
        float z = Mathf.Round(pos.z);

        transform.position = new Vector3(x, pos.y + 5f, z);
    }
}
