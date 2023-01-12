using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGizmos;  //自作名前空間：ギズモを追加する

public class SightController : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField, Range(0f, 1f), Tooltip("1のとき：オブジェクトの真正面、0のとき:オブジェクトの真右(真左)")]
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
                Debug.Log("LOOK");
            }
            else
            {
                Debug.Log("LOST");
            }
        }
    }

    /// <summary>索敵する</summary>
    /// <param name="target">索敵対象</param>
    /// <param name="angle">索敵範囲</param>
    /// <param name="range">索敵距離</param>
    /// <returns>true -> 発見した | false -> 発見していない</returns>
    private bool TargetSearch(Transform target, float angle, float range)
    {
        // ターゲットがどの方向に居るかを計算する。
        Vector3 toTarget = (target.position - transform.position).normalized;
        Debug.DrawRay(transform.position, toTarget * range, Color.cyan);

        // 「自分の正面とターゲットがいる方向の内積」と「ターゲットとの距離」を計算する。
        float dot = Vector3.Dot(transform.forward, toTarget);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 計算した内積と距離が一定範囲内だったら、発見したことにする。
        if (dot >= angle && distanceToTarget <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary></summary>
    /// <param name="target"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private bool TargetLook(Transform target, float range)
    {
        // Playerを凝視する。
        _turnAroundDir = transform.forward;
        transform.LookAt(target);

        // Rayを飛ばして自身とターゲットの間に障害物があるかどうか確認する。
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range);
        Debug.DrawRay(transform.position, transform.forward * range, Color.red);

        // HitしたColliderのTagがPlayer以外だったら、凝視をやめる。
        if (hit.collider.tag != "Player")
        {
            transform.LookAt(null);
            transform.forward = _turnAroundDir;
            return false;
        }
        else
        {
            return true;
        }
    }
}
