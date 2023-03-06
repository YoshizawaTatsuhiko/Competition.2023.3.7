using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class TitleCameraController : MonoBehaviour
{
    [SerializeField]
    private float _radius = 1f;
    private float _degree = 0f;
    private Vector3 _pos = Vector3.zero;

    private void FixedUpdate()
    {
        transform.position = new Vector3(
            _pos.x + _radius * Mathf.Cos(_degree),
            transform.position.y,
            _pos.z + _radius * Mathf.Sin(_degree));
        _degree += 2 * Mathf.PI / 360f * 0.5f;
    }
}
