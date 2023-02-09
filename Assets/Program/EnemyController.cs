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
    [SerializeField]
    private float _range = 1f;
    /// <summary>ウロウロするときに向く方向</summary>
    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _sight = GetComponent<SightController>();
    }

    private bool _isChangeDirection = true;

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
            WanderMove();
        }
        //else  // ターゲットが発見できていないとき
        //{
        //    if (_isChangeDirection) StartCoroutine(Wander());
        //}
        _rb.velocity = transform.forward * _moveSpeed;
        Debug.DrawRay(transform.position, transform.forward * _range, Color.cyan);
    }

    ///// <summary>オブジェクトがウロウロする</summary>
    //private IEnumerator Wander()
    //{
    //    Debug.Log("Coroutine Start");
    //    _isChangeDirection = false;
    //    Vector3 random = Random.insideUnitSphere;
    //    _direction = new Vector3(random.x, 0f, random.z).normalized;
    //    transform.forward = _direction;

    //    yield return new WaitForSeconds(_waitTime);
    //    _isChangeDirection = true;
    //    Debug.Log("Coroutine Finish");
    //}

    private void WanderMove()
    {
        Vector3 random = Random.insideUnitSphere;
        _direction = new Vector3(random.x, 0f, random.z).normalized;
        transform.forward = _direction;
    }
}
