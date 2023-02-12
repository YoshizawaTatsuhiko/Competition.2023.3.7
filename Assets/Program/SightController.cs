using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField]
    private float _searchDegree = 30f;
    [SerializeField]
    private float _searchRange = 15f;

    private Vector3 _lineOfSight = Vector3.zero;

    /// <summary>ターゲットが視界内に入っているか判定する</summary>
    /// <param name="target">判定するターゲット</param>
    /// <param name="degree">視野角(度数法)</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>true -> 発見 | false -> 未発見 or 見失った</returns>
    public bool SearchTarget()
    {
        // ターゲットのいる方向
        Vector3 toTarget = _target.position - transform.position;
        // 自身の正面を 0°として、180°まで判定すればいいので、cos(視野角/2)を求める。
        float cosHalf = Mathf.Cos(_searchDegree / 2 * Mathf.Deg2Rad);
        // 自身の正面とターゲットがいる方向とのcosθの値を計算する。
        float cosAngle = Vector3.Dot(transform.forward, toTarget) / (transform.forward.magnitude * toTarget.magnitude);
        // ターゲットが視界範囲内に入っているかの結果を返す。
        return cosAngle >= cosHalf && toTarget.magnitude < _searchRange;
    }

    /// <summary>ターゲットとの間に障害物があるか判定する</summary>
    /// <param name="target">判定するターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>true -> 障害物ナシ | false -> 障害物アリ</returns>
    public bool LookTarget()
    {
        // ターゲットを見失った時に見る方向。
        _lineOfSight = transform.forward;
        // ターゲットを凝視する。
        transform.LookAt(Vector3.Lerp(transform.forward + transform.position, _target.position, 0.2f));
        // ターゲットとの間の障害物があるかを調べるためにRaycastを飛ばす。
        Debug.DrawRay(transform.position, transform.forward * _searchRange, Color.white);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _searchRange))
        {
            if (hit.collider.tag == $"{ _target.gameObject.tag }")
            {
                return true;
            }
            else
            {
                transform.LookAt(null);
                transform.forward = _lineOfSight;
                return false;
            }
        }

        return false;
    }
}
