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
    /// <summary>Goalを出現するために起動するGimmick</summary>
    private GimmickCheck _gimmickCheck = null;
    private GoalController _goal = null;

    private void Start()
    {
        // ゲーム開始時にマウスカーソルを見えなくする。
        Cursor.visible = false;

        // ゲーム開始地点の座標を見つけておく
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        _startPoint.y += 1f;

        // ResourcesフォルダーからPlayerを生成する。
        Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Marker"));

        _gimmickCheck = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        _timeLimit = Mathf.Clamp(_timeLimit -= Time.fixedDeltaTime, 0f, _timeLimit);
        _timeText.text = _timeLimit.ToString("F2");

        // 起動したGimmickの数が一定数に達したら、Goalを出現させる。
        if (_gimmickCheck.GimmickWakeUpCount == 2)
        {
            _goal.gameObject.SetActive(true);
        }
        // Playerがゴールに到着したら、ゲームクリア
        if (_goal.GoalJudge) GameClear();
        // 制限時間がなくなったら、ゲームオーバー
        if (_timeLimit == 0f) GameOver();
    }

    /// <summary>ゲームクリアした時にEventを呼ぶ</summary>
    private void GameClear()
    {
        _gameClear.Invoke();
        Cursor.visible = true;
        Debug.Log("Complete");
    }

    /// <summary>ゲームオーバーした時にEventを呼ぶ</summary>
    private void GameOver()
    {
        Cursor.visible = true;
        _gameOver.Invoke();
        Debug.Log("Mission Failed");
    }
}
