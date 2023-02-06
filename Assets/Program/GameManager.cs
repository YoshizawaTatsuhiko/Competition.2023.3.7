using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _gameClear = new();
    [SerializeField]
    private UnityEvent _gameOver = new();
    private GameObject _player = null;
    /// <summary>ゲームが始まった時にPlayerを生成する座標</summary>
    private Vector3 _startPoint = new();
    /// <summary>Goalを出現するために起動するGimmick</summary>
    private GimmickCheck _gimmick = null;
    private GoalController _goal = null;

    private void Start()
    {
        // ゲーム開始時にマウスカーソルを見えなくする。
        Cursor.visible = false;

        // ゲーム開始地点の座標を見つけておく
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        _startPoint.y += 1f;

        // ResourcesフォルダーからPlayerを生成する。
        _player = Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Marker"));

        _gimmick = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // 起動したGimmickの数が一定数に達したら、Goalを出現させる。
        if (_gimmick.GimmickWakeUpCount == 2)
        {
            _goal.gameObject.SetActive(true);
        }

        if (_goal.GoalJudge)
        {
            GameClear();
        }
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
        _gameOver.Invoke();
    }
}
