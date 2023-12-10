using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private MazeGenerator _mazeGenerator = null;
    [SerializeField]
    private bool _isUnlimitedTime = false;
    [SerializeField]
    private float _timeLimit = 60f;
    [SerializeField]
    private Text _timeText = null;
    [SerializeField]
    private Text _lifeText = null;
    [SerializeField]
    private UnityEvent _gameClear = new();
    [SerializeField]
    private UnityEvent _gameOver = new();
    /// <summary>ゲームが始まった時にPlayerを生成する座標</summary>
    private Vector3 _startPoint = Vector3.zero;
    private PlayerController _player = null;
    private MouseSensitivity _mouseSensitivity = null;
    private LifeController _playerLife = null;
    /// <summary>Goalを出現するために起動するGimmick</summary>
    private GimmickCheck _gimmickCheck = null;
    private GoalController _goal = null;
    /// <summary>Pauseを登録するイベント</summary>
    private event Action _onPause = null;
    public event Action OnPause { add => _onPause += value; remove => _onPause -= value; }
    /// <summary>Pauseを解除するイベント</summary>
    private event Action _onResume = null;
    public event Action OnResume { add => _onResume += value; remove => _onResume -= value; }
    [SerializeField]
    private GameObject _option = null;

    private void Awake()
    {
        _mazeGenerator = FindObjectOfType<MazeGenerator>();
        _mazeGenerator.GenerateMazeFromBlueprint();

        // Playerを生成する座標を見つけておく。
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        if (_startPoint == null) _startPoint = Vector3.zero;
        _startPoint.y += 1f;
    }

    private void OnEnable()
    {
        _player = FindObjectOfType<PlayerController>();

        if (_player != null) _player.transform.position = _startPoint;

        if (_player == null)
        {
            // ResourcesフォルダーからPlayerを生成する。
            _player = Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity)
                .GetComponentInChildren<PlayerController>();
        }
        _playerLife = _player.GetComponent<LifeController>();
        Instantiate(Resources.Load<GameObject>("Marker"));

        _mouseSensitivity = FindObjectOfType<MouseSensitivity>();
        _mouseSensitivity.Player = _player;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _gimmickCheck = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal?.gameObject.SetActive(false);

        if (_isUnlimitedTime) _timeLimit = Mathf.Infinity;

        if (_option) _option.SetActive(false);
    }

    private bool _isGameFinish = false;
    private int _pushCount = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pushCount++;

            if (_pushCount % 2 == 1)
            {
                _onPause?.Invoke();
                _option?.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _mouseSensitivity.ViewLock();
            }
            else
            {
                _onResume?.Invoke();
                _option?.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _mouseSensitivity.ViewUnlock();
            }
            if (_pushCount >= 100) _pushCount = 0;
        }

        if (!_isGameFinish)
        {
            _timeLimit = Mathf.Clamp(_timeLimit -= Time.deltaTime, 0f, _timeLimit);
            if (_timeText) _timeText.text = _timeLimit.ToString("F2");
        }

        if (_lifeText) _lifeText.text = $"Life:{_playerLife.CurrentLife}";

        // 起動したGimmickの数が一定数に達したら、Goalを出現させる。
        if (_gimmickCheck?.GimmickWakeUpCount == 2)
        {
            _goal?.gameObject.SetActive(true);
        }
        // Playerがゴールに到着したら、ゲームクリア
        if (_goal.GoalJudge) GameClear();
        // 制限時間 or PlayerのHPがなくなったら、ゲームオーバー
        if (_timeLimit == 0f || _playerLife.CurrentLife <= 0f) GameOver();
    }

    /// <summary>ゲームクリアした時に呼ぶEvent</summary>
    private void GameClear()
    {
        _gameClear.Invoke();
        CommonBehaviour();
        Debug.Log("Complete");
    }

    /// <summary>ゲームオーバーした時に呼ぶEvent</summary>
    public void GameOver()
    {
        _gameOver.Invoke();
        CommonBehaviour();
        Debug.Log("Mission Failed");
    }

    /// <summary>GameClear()、GameOver()に共通する処理</summary>
    private void CommonBehaviour()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _isGameFinish = true;
        _onPause?.Invoke();
        _mouseSensitivity.ViewLock();
    }
}