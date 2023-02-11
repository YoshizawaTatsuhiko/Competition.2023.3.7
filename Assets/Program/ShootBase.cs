using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

// 日本語対応
public class ShootBase : MonoBehaviour
{
    private LineRenderer _line = null;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
    }

    /// <summary>始点から終点に向かって、光線を描く</summary>
    /// <param name="origin">始点</param>
    /// <param name="destination">終点</param>
    public void DrawRay(Vector3 origin, Vector3 destination)
    {
        Vector3[] positions = { origin, destination };
        _line.positionCount = positions.Length;
        _line.SetPositions(positions);
    }
}
