using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SightController))]

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1f;
    private Rigidbody _rb = null;
    private SightController _sight = null;
    [SerializeField]
    private float _waitTime = 3f;
    /// <summary>ウロウロするときに向く方向</summary>
    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _sight = GetComponent<SightController>();
    }

    private float _timer = 0f;
    private bool _isChangeDirection = true;

    private void FixedUpdate()
    {
        if (_sight.SearchTarget())  // ターゲットを発見したとき
        {
            if (!_sight.LookTarget())  // ターゲットとの間に障害物がある時
            {
                _timer += Time.fixedDeltaTime;
                if (_timer > _waitTime)  // ターゲットが障害物に一定時間隠れたら、索敵しなおす。
                {
                    _timer = 0f;
                    return;
                }
            }
        }
        else  // ターゲットが発見できていないとき
        {
            if(_isChangeDirection) StartCoroutine(Wander());
        }
        _rb.velocity = transform.forward * _moveSpeed;
    }

    /// <summary>オブジェクトがウロウロする</summary>
    private IEnumerator Wander()
    {
        _isChangeDirection = false;
        Vector3 random = Random.insideUnitSphere;
        _direction = new Vector3(random.x, 0f, random.z).normalized;
        transform.forward = _direction;

        yield return new WaitForSeconds(_waitTime);
        _isChangeDirection = true;
    }
}
