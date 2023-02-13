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
    private float _distanceToTarget = 0.5f;
    [SerializeField]
    private float _range = 1f;
    /// <summary>ウロウロするときに向く方向</summary>
    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _sight = GetComponent<SightController>();
    }

    private void FixedUpdate()
    {
        if (_sight.SearchTarget())  // ターゲットを発見したとき
        {
            if (!_sight.LookTarget())  // ターゲットとの間に障害物がある時
            {
                return;  // ターゲットが障害物に隠れたら、索敵しなおす。
            }
        }
        else if(Physics.Raycast(transform.position, transform.forward, _range))
        {
            Vector3 random = Random.insideUnitSphere;
            _direction = new Vector3(random.x, 0f, random.z).normalized;
            transform.forward = _direction;
        }
        Debug.DrawRay(transform.position, transform.forward * _range, Color.cyan);

        if (Vector2.Distance(transform.position, _sight.Target.position) >= _distanceToTarget)
        {
            _rigidbody.velocity = new Vector3(
                transform.forward.x * _moveSpeed, _rigidbody.velocity.y, transform.forward.z * _moveSpeed);
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
