using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyGizmos;  //自作名前空間：Gizmosを追加する

public class SightController : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;
    [SerializeField, Range(0f, 1f)]
    private float _searchDeg = 1f;
    [SerializeField]
    private float _searchDistance = 1f;
    // ターゲットとの距離
    private float _distanceToTarget = 0f;
    // ターゲットを発見したかどうかを判定する
    private bool _isFind = false;
    // ターゲットを見失ったかどうかを判定する
    private bool _isTergetLost = false;
    // ターゲットを見失った時に向き直る方向
    private Vector3 _turnAroundDir = Vector3.zero;

    private void Start()
    {
        if (!_target) Debug.LogWarning("対象がassignされていません。");
    }

    private void FixedUpdate()
    {
        Vector3 toTarget = transform.position - _target.position;
        float dot = Vector3.Dot(transform.forward, toTarget);
    }
}
