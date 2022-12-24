using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SightController : MonoBehaviour
{
    [SerializeField, Tooltip("索敵する対象")]
    private Transform _terget;
    [SerializeField, Tooltip("索敵範囲")]
    private float _searchAngle = 1f;
    [SerializeField, Tooltip("索敵距離")]
    private float _range = 1f;
    [SerializeField, Tooltip("Gizmosを表示するかどうか\ntrue -> 表示 | false -> 非表示")]
    private bool _isGizmos = true;
    /// <summary>標的のいる方向</summary>
    private Vector3 _direction;

    private void FixedUpdate()
    {
        if (SearchTerget())
        {
            Debug.Log("お前を見ている");
        }
        else
        {
            Debug.Log("(暇だなぁ...)");
        }
    }

    /// <summary>Tergetが視界に入っているかどうか判定する</summary>
    /// <returns>入っている -> true | 入っていない -> false;</returns>
    public bool SearchTerget()
    {
        //Tergetとゲームオブジェクトの距離や方向を計算する
        float distance = Vector3.Distance(_terget.position, transform.position);
        _direction = _terget.position - transform.position;
        float angle = Vector3.Angle(_direction, transform.forward);

        //Playerが視界の範囲内に入った時の処理
        if (angle <= _searchAngle && distance <= _range)
        {
            transform.forward = _direction;
            return true;
        }

        //Playerが視界の範囲外に居る時の処理
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        //_isGizmosがfalseの時、Gizmosを非表示にする
        if (_isGizmos == false) return;

        //Playerを検知するRay
        Gizmos.DrawRay(transform.position, _direction * _range);

        //視界を表示する
        Color color = new Color(1f, 0f, 0f, 0.2f);
        //Handles.color = color;
        //Handles.DrawSolidArc(transform.position, transform.forward,
        //    transform.forward * _searchAngle,
        //    Mathf.PI * 2 * Mathf.Rad2Deg, _range);
        MyGizmos.AddGizmos.DrawWireCone(transform.position, transform.forward * _range, Vector3.up, _searchAngle);
    }
}
