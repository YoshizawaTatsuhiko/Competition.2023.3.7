using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField, Range(0f, 180f)]
    private float _searchDegree = 30f;
    [SerializeField]
    private float _searchRange = 15f;
    /// <summary>ターゲットを見失った時に見る方向</summary>
    private Vector3 _turnAroundDir = Vector3.zero;

    private void Start()
    {
        if (!_target) Debug.LogWarning("ターゲットがassignされていません。");
    }

    private void FixedUpdate()
    {
        if (SearchTarget(_target, _searchDegree, _searchRange))
        {
            if (LookTarget(_target, _searchRange))
            {
                Debug.DrawRay(transform.position, transform.forward * _searchRange, Color.red);
                Debug.Log("LOOK");
            }
            else
            {
                Debug.Log("LOST");
            }
        }
    }

    /// <summary>ターゲットが視界内に入っているか判定する</summary>
    /// <param name="target">判定するターゲット</param>
    /// <param name="degree">視野角(度数法)</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>true -> 発見 | false -> 未発見 or 見失った</returns>
    private bool SearchTarget(Transform target, float degree, float range)
    {
        // ターゲットのいる方向
        Vector3 toTarget = target.position - transform.position;
        // 自身の正面を 0°として、180°まで判定すればいいので、cos(視野角/2)を求める。
        float cosHalf = Mathf.Cos(degree / 2 * Mathf.Deg2Rad);
        // 自身の正面とターゲットがいる方向とのcosθの値を計算する。
        float cosAngle = Vector3.Dot(transform.forward, toTarget) / (transform.forward.magnitude * toTarget.magnitude);
        // ターゲットが視界範囲内に入っているかの結果を返す。
        return cosAngle >= cosHalf && toTarget.magnitude < range;
    }

    /// <summary>ターゲットとの間に障害物があるか判定する</summary>
    /// <param name="target">判定するターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>true -> 障害物ナシ | false -> 障害物アリ</returns>
    private bool LookTarget(Transform target, float range)
    {
        // ターゲットを見失った時に見る方向。
        _turnAroundDir = transform.forward;
        // ターゲットを凝視する。
        transform.LookAt(target);
        // ターゲットとの間の障害物があるかを調べるためにRaycastを飛ばす。
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range);

        if (hit.collider.tag == $"{target.gameObject.tag}")
        {
            return true;
        }
        else
        {
            transform.LookAt(null);
            transform.forward = _turnAroundDir;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * _searchRange);
    }
}
