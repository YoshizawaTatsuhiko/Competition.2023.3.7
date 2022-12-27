using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MyGizmos;  //自作名前空間：Gizmosを追加する

public class SightController : MonoBehaviour
{
    [SerializeField, Tooltip("索敵する対象")]
    private Transform _terget;
    [SerializeField, Range(0f, 1f), Tooltip("索敵範囲")]
    private float _searchAngle = 1f;
    [SerializeField, Tooltip("索敵距離")]
    private float _range = 1f;
    //[SerializeField, Tooltip("Gizmosを表示するかどうか\ntrue -> 表示 | false -> 非表示")]
    //private bool _isGizmos = true;
    /// <summary>標的のいる方向</summary>
    private float _distance;

    private void Start()
    {
        if (!_terget) Debug.LogWarning("ターゲットがassignされていません。");
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_terget.transform.position, transform.position);

        if (SearchTerget(_searchAngle) && _distance <= _range)
        {
            Debug.Log("通過");
            if (LookAtTerget())
            {
                Debug.Log("お前を見ている");
            }
        }
        else
        {
            Debug.Log("(暇だなぁ...)");
        }
    }

    /// <summary>Tergetが視界に入っているかどうか判定する</summary>
    /// <param name="angle">索敵範囲</param>
    /// <returns>入っている -> true | 入っていない -> false;</returns>
    private bool SearchTerget(float angle)
    {
        //Tergetとゲームオブジェクトの内積を計算する
        float dot = Vector3.Dot(transform.forward, (_terget.transform.position - transform.position).normalized);

        //Playerが視界の範囲内に入った時の処理
        if(dot > angle)
        {
            return true;
        }

        //Playerが視界の範囲外に居る時の処理
        else
        {
            return false;
        }
    }

    /// <summary>ターゲットとゲームオブジェクトの間に障害物があるかどうかを判定する</summary>
    /// <returns>障害物ナシ -> true | 障害物アリ -> false</returns>
    private bool LookAtTerget()
    {
        Debug.DrawRay(transform.position, transform.forward * _range);

        if (Physics.Raycast(transform.position, transform.forward, _range, _terget.gameObject.layer))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
