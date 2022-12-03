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
    [SerializeField, Tooltip("")]
    private bool _isGizmos = true;
    /// <summary>Playerを認識中かどうか判別する</summary>
    private bool _isDiscover = false;
    /// <summary>Playerのいる方向</summary>
    private Vector3 _direction;

    private void FixedUpdate()
    {
        //Playerとゲームオブジェクトの距離や方向を計算する
        float distance = Vector3.Distance(_terget.position, transform.position);
        _direction = _terget.position - transform.position;
        float angle = Vector3.Angle(_direction, transform.forward);

        //Playerが視界の範囲内に入った時の処理
        if (angle <= _searchAngle && distance <= _range)
        {
            transform.forward = _terget.position;
            _isDiscover = Physics.Raycast(transform.position, 
                                                  _direction, _range, LayerMask.GetMask("Player"));

            if (_isDiscover)
            {

            }
            else
            {

            }
        }
    }

    private void OnDrawGizmos()
    {
        //_isGizmosがfalseの時、Gizmosを非表示にする
        if (_isGizmos == false) return;

        //Playerを検知するRay
        Gizmos.DrawRay(transform.position, _direction * _range);
        Handles.color = Color.red;
        //Handles.DrawSolidArc(transform.position, Vector3.up,);
    }
}
