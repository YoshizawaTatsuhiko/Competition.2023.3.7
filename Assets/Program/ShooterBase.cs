using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

// 日本語対応
public class ShooterBase : MonoBehaviour
{
    /// <summary>銃弾の軌跡を表す光線(Coroutine)</summary>
    /// <param name="origin">光線の始点</param>
    /// <param name="destination">光線の終点</param>
    public IEnumerator DrawLaser(Vector3 origin, Vector3 destination)
    {
        if (gameObject.TryGetComponent(out LineRenderer lineRenderer))
        {
            LineRenderer line = lineRenderer;
            Vector3[] positions = { origin, destination };
            line.positionCount = positions.Length;
            line.SetPositions(positions);
            yield return new WaitForSeconds(0.1f);
            line.SetPosition(1, origin);
        }
    }
}
