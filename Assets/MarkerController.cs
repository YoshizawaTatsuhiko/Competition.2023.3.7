using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    [SerializeField]
    private float _height = 1f;
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

        float x = pos.x;
        float z = pos.z;

        if (x > 0)
        {
            x = Mathf.Floor(x + 0.5f);
            z = Mathf.Floor(z + 0.5f);
        }
        else
        {
            x = Mathf.Ceil(x - 0.5f);
            z = Mathf.Ceil(z - 0.5f);
        }

        transform.position = new Vector3(x, _height, z);
    }
}
