using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SightController))]

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1f;
    private Rigidbody _rigidbody = null;
    private SightController _sight = null;
    [SerializeField]
    private float _range = 1f;
    [SerializeField]
    private LayerMask _layerMask = 0;
    [SerializeField]
    private int[] _rotateDegree = new int[0];
    private float _saveSpeed;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _sight = GetComponent<SightController>();
        _saveSpeed = _moveSpeed;
    }

    private void FixedUpdate()
    {
        if (_sight.SearchTarget())  // ターゲットを発見したとき
        {
            // ターゲットを見つけたら、動きを止める。
            _moveSpeed = 0f;

            if (!_sight.LookTarget())  // ターゲットとの間に障害物がある時
            {
                return;  // ターゲットが障害物に隠れたら、索敵しなおす。
            }
        }
        else if(Physics.Raycast(transform.position, transform.forward, _range, _layerMask))  // 障害物に一定距離まで近づいた時
        {
            // ランダムに方向を決め、その方向に向き直る。
            int randomIndex = Random.Range(0, _rotateDegree.Length);
            transform.rotation = Quaternion.AngleAxis(_rotateDegree[randomIndex], Vector3.up);
            Debug.DrawRay(transform.position, transform.forward * _range, Color.cyan);
        }
        else  // 何も起きていない時 or ターゲットを見失った時。
        {
            _moveSpeed = _saveSpeed;  // 移動速度を元通りにする。
        }

        _rigidbody.velocity = new Vector3(
                transform.forward.x * _moveSpeed, _rigidbody.velocity.y, transform.forward.z * _moveSpeed);
    }
}
