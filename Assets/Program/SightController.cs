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
    /// <summary>対象との距離</summary>
    private float _distance;
    /// <summary>対象を発見したかどうかを判定する</summary>
    private bool _isDiscover = false;
    /// <summary>対象見失ったかどうかを判定する</summary>
    private bool _isTergetLost = false;
    /// <summary>対象を見失った時に向き直る方向</summary>
    private Vector3 _front = Vector3.zero;
    /// <summary>振り向くまでの時間</summary>
    private float _turnTime = 0.5f;

    private void Start()
    {
        if (!_terget) Debug.LogWarning("対象がassignされていません。");
    }

    private void FixedUpdate()
    {
        //_distance = Vector3.Distance(_terget.transform.position, transform.position);

        //対象を発見した時
        if(_isDiscover)
        {
            _front = transform.forward;

            //Rayを飛ばして、対象との間に障害物があるかどうか調べる
            if (Physics.Raycast(transform.position, transform.forward * _range, _terget.gameObject.layer))
            {
                Debug.DrawRay(transform.position, transform.forward * _range);
                transform.LookAt(Vector3.Lerp(transform.forward + transform.position, _terget.position, _turnTime));
                Debug.Log("Look...");
            }
            else
            {
                _isDiscover = false;
                _isTergetLost = true;
                Debug.Log("Switch");
            }
        }

        //対象を発見できていない時
        else
        {
            //対象を発見するときに使う内積
            float discoverDot = Mathf.Abs(Vector3.Dot(
                transform.forward, (_terget.position - transform.position).normalized));

            if (discoverDot >= _searchAngle)
            {
                _isDiscover = true;
                Debug.Log("Discover!");
            }
            if (_isTergetLost)
            {
                //対象を見失った時、発見する以前の方向を向くときに使う内積
                float lookDot = Vector3.Dot(transform.forward, _front);

                if (lookDot <= 0.95f)
                {
                    transform.LookAt(
                        Vector3.Lerp(transform.forward + transform.position, _front + transform.position, _turnTime));
                }
                else  //ある程度発見前の方向を向いたら、振り向くのをやめる
                {
                    _isTergetLost = false;
                }
            }
            //Debug.Log("(暇だなぁ...)");
        }
    }

    /*
    /// <summary>Tergetが視界に入っているかどうか判定する</summary>
    /// <param name="angle">索敵範囲</param>
    /// <returns>入っている -> true | 入っていない -> false;</returns>
    private bool SearchTerget(float angle)
    {
        //Tergetとゲームオブジェクトの内積を計算する
        float dot = Vector3.Dot(transform.forward, (_terget.position - transform.position).normalized);

        //Playerが視界の範囲内に入った時の処理
        if(Mathf.Abs(dot) >= angle)
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
    /// <param name="terget">凝視する対象</param>
    /// <returns>障害物ナシ -> true | 障害物アリ -> false</returns>
    private void LookAtTerget(Transform terget)
    {
        //ターゲットの方向を向く
        transform.LookAt(Vector3.Lerp(transform.forward + transform.position, terget.position, 1f));

        //対象とゲームオブジェクトの間に障害物があるかどうかを確認するためのRayを飛ばす
        Physics.Raycast(transform.position, transform.forward, terget.gameObject.layer);
        Debug.DrawRay(transform.position, transform.forward);
    }
    */
}
