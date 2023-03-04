using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SightController))]

public class EnemyController : MonoBehaviour, IPauseResume
{
    [SerializeField]
    private float _moveSpeed = 1f;
    private Rigidbody _rigidbody = null;
    private SightController _sight = null;
    [SerializeField]
    private float _range = 1f;
    [SerializeField]
    private LayerMask _layerMask = 0;
    private EnemyShootController _enemyShooter = null;
    [SerializeField]
    private int[] _rotateDegree = new int[0];
    private float _saveSpeed;

    //=====Pause Resume 用変数=====
    private GameManager _gameManager = null;
    private Vector3 _tempSaveVelocity = Vector3.zero;
    private Vector3 _tempSaveAngularVelocity = Vector3.zero;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnPause += Pause;
        _gameManager.OnResume += Resume;
    }

    private void OnDisable()
    {
        _gameManager.OnPause -= Pause;
        _gameManager.OnResume -= Resume;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _sight = GetComponent<SightController>();
        _saveSpeed = _moveSpeed;
        _enemyShooter = FindObjectOfType<EnemyShootController>();
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
            _enemyShooter.EnemyShooter();
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

    public void Pause()
    {
        _tempSaveVelocity = _rigidbody.velocity;
        _tempSaveAngularVelocity = _rigidbody.angularVelocity;
        _rigidbody.Sleep();
        _rigidbody.isKinematic = true;
    }

    public void Resume()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.WakeUp();
        _rigidbody.angularVelocity = _tempSaveAngularVelocity;
        _rigidbody.velocity = _tempSaveVelocity;
    }
}
