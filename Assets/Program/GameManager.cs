using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timeLimit = 60f;
    [SerializeField]
    private Text _timeText = null;
    [SerializeField]
    private UnityEvent _gameClear = new();
    [SerializeField]
    private UnityEvent _gameOver = new();
    /// <summary>ゲームが始まった時にPlayerを生成する座標</summary>
    private Vector3 _startPoint = new();
    private LifeController _playerHP = new LifeController();
    /// <summary>Goalを出現するために起動するGimmick</summary>
    private GimmickCheck _gimmickCheck = null;
    private GoalController _goal = null;
    /// <summary>Pauseを登録するイベント</summary>
    private event Action _onPause = null;
    public event Action OnPause { add => _onPause += value; remove => _onPause -= value; }
    /// <summary>Pauseを解除するイベント</summary>
    private event Action _onResume = null;
    public event Action OnResume { add => _onResume += value; remove => _onResume -= value; }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Playerを生成する座標を見つけておく。
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        if (_startPoint == null) _startPoint = Vector3.zero;
        _startPoint.y += 1f;

        // ResourcesフォルダーからPlayerを生成する。
        GameObject player = Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity);
        if (player.TryGetComponent(out LifeController hp)) _playerHP = hp;
        Instantiate(Resources.Load<GameObject>("Marker"));

        _gimmickCheck = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal?.gameObject.SetActive(false);
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
            }
            else
            {
                _onResume?.Invoke();
            }
            if (_pushCount >= 10) _pushCount = 0;
        }

        if (!_isGameFinish)
        {
            _timeLimit = Mathf.Clamp(_timeLimit -= Time.fixedDeltaTime, 0f, _timeLimit);
            if (_timeText) _timeText.text = _timeLimit.ToString("F2");
        }

        // 起動したGimmickの数が一定数に達したら、Goalを出現させる。
        if (_gimmickCheck?.GimmickWakeUpCount == 2)
        {
            _goal?.gameObject.SetActive(true);
        }
        // Playerがゴールに到着したら、ゲームクリア
        if (_goal.GoalJudge) GameClear();
        // 制限時間 or PlayerのHPがなくなったら、ゲームオーバー
        if (_timeLimit == 0f || _playerHP.CurrentLife <= 0f) GameOver();
    }

    /// <summary>ゲームクリアした時に呼ぶEvent</summary>
    private void GameClear()
    {
        _gameClear.Invoke();
        CommonBehaviour();
        Debug.Log("Complete");
    }

    /// <summary>ゲームオーバーした時に呼ぶEvent</summary>
    private void GameOver()
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
    }
}