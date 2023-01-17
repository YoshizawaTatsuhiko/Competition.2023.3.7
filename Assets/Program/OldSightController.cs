using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGizmos;  //自作名前空間：ギズモを追加する

public class OldSightController : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField, Range(0f, 1f), Tooltip("0 -> 1 になるほど、視界が狭くなる")]
    private float _searchAngle = 1f;
    [SerializeField]
    private float _searchRange = Mathf.Infinity;
    /// <summary>ターゲットを見失った時に向き直る方向</summary>
    private Vector3 _turnAroundDir = Vector3.zero;

    private void Start()
    {
        if (!_target) Debug.LogWarning("対象がassignされていません。");
    }

    private void FixedUpdate()
    {
        if (TargetSearch(_target, _searchAngle, _searchRange))
        {
            if (TargetLook(_target, _searchRange))
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

    /// <summary>索敵する</summary>
    /// <returns>true -> 発見した | false -> 発見していない</returns>
    private bool TargetSearch(Transform target, float degree, float range)
    {
        // ターゲットがどの方向に居るかを計算する。
        Vector3 toTarget = (target.position - transform.position).normalized;
        Debug.DrawRay(transform.position, toTarget * range, Color.cyan);

        // 「自分の正面とターゲットがいる方向の内積」と「ターゲットとの距離」を計算する。
        float dot = Vector3.Dot(transform.forward, toTarget);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 計算した内積と距離が一定範囲内だったら、発見したことにする。
        return dot >= degree && distanceToTarget <= range;
    }

    /// <summary>ターゲットとの間に障害物があるかどうか調べる</summary>
    /// <returns>true -> 障害物ナシ | false -> 障害物アリ</returns>
    private bool TargetLook(Transform target, float range)
    {
        // Playerを凝視する。
        _turnAroundDir = transform.forward;
        transform.LookAt(target);

        // Rayを飛ばして自身とターゲットの間に障害物があるかどうか確認する。
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range);

        // HitしたColliderのTagがPlayer以外だったら、凝視をやめる。
        if (hit.collider.tag == "Player")
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
        AddGizmos.DrawWireCone(transform.position, transform.forward, _searchRange, _searchAngle);
    }
}
