using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    private HPController _playerHP = new HPController();
    /// <summary>Goalを出現するために起動するGimmick</summary>
    private GimmickCheck _gimmickCheck = null;
    private GoalController _goal = null;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Playerを生成する座標を見つけておく。
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        _startPoint.y += 1f;

        // ResourcesフォルダーからPlayerを生成する。
        GameObject player = Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity);
        if (player.TryGetComponent(out HPController hp)) _playerHP = hp;
        Instantiate(Resources.Load<GameObject>("Marker"));

        _gimmickCheck = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal?.gameObject.SetActive(false);
    }

    private bool _isGameFinish = false;

    private void Update()
    {
        if (!_isGameFinish)
        {
            _timeLimit = Mathf.Clamp(_timeLimit -= Time.fixedDeltaTime, 0f, _timeLimit);
        }
        if(_timeText) _timeText.text = _timeLimit.ToString("F2");

        // 起動したGimmickの数が一定数に達したら、Goalを出現させる。
        if (_gimmickCheck?.GimmickWakeUpCount == 2)
        {
            _goal?.gameObject.SetActive(true);
        }
        // Playerがゴールに到着したら、ゲームクリア
        if (_goal.GoalJudge) GameClear();
        // 制限時間 or PlayerのHPがなくなったら、ゲームオーバー
        if (_timeLimit == 0f || _playerHP.CurrentHP == 0f) GameOver();
    }

    /// <summary>ゲームクリアした時にEventを呼ぶ</summary>
    private void GameClear()
    {
        _gameClear.Invoke();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _isGameFinish = true;
        Debug.Log("Complete");
    }

    /// <summary>ゲームオーバーした時にEventを呼ぶ</summary>
    private void GameOver()
    {
        _gameOver.Invoke();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _isGameFinish = true;
        Debug.Log("Mission Failed");
    }
}
