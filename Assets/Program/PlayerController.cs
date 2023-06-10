using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour, IPauseResume
{
    [SerializeField,Tooltip("移動速度")]
    private float _moveSpeed = 1.0f;
    private Rigidbody _rigidbody = null;
    private PlayerShootController _playerShooter = null;
    private GimmickCheck _gimmickCheck = null;

    //=====Pause Resume 用変数=====
    private GameManager _gameManager = null;
    private Vector3 _tempSaveVelocity = Vector3.zero;
    private Vector3 _tempSaveAngularVelocity = Vector3.zero;
    private bool _isPause = false;

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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        if (TryGetComponent(out PlayerShootController PSC)) _playerShooter = PSC;
        if (TryGetComponent(out GimmickCheck GC)) _gimmickCheck = GC;
    }

    void FixedUpdate()
    {
        // Playerの操作系統の処理
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")) * _moveSpeed;
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;

        // Playerの視点操作系統の処理
        Vector3 camDir = Camera.main.transform.forward;
        camDir.y = 0;
        transform.forward = camDir;
    }

    private void Update()
    {
        if (!_isPause)
        {
            _playerShooter.PlayerShooter();
            _gimmickCheck.GimmickJudgement();
        }
    }

    public void Pause()
    {
        _isPause = true;
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
        _isPause = false;
    }
}
