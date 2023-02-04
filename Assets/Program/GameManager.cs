using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private GameObject _player = null;
    /// <summary>ÉQÅ[ÉÄÇ™énÇ‹Ç¡ÇΩéûÇ…PlayerÇê∂ê¨Ç∑ÇÈç¿ïW</summary>
    private Vector3 _startPoint = new();
    [SerializeField]
    private UnityEvent _gameClear = new();
    [SerializeField]
    private UnityEvent _gameOver = new();

    private GimmickCheck _gimmick = null;
    private GoalController _goal = null;

    private void Start()
    {
        Cursor.visible = false;
        _startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;
        _startPoint.y += 1f;
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            _player = Instantiate(Resources.Load<GameObject>("Player Prefab"), _startPoint, Quaternion.identity);
        }
        Instantiate(Resources.Load<GameObject>("Marker"));
        _gimmick = FindObjectOfType<GimmickCheck>();
        _goal = FindObjectOfType<GoalController>();
        _goal.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_gimmick.GimmickWakeUpCount == 2)
        {
            _goal.gameObject.SetActive(true);
        }

        if (_goal.GoalJudge)
        {
            GameClear();
        }
    }

    private void GameClear()
    {
        _gameClear.Invoke();
        Debug.Log("Complete");
    }

    private void GameOver()
    {
        _gameOver.Invoke();
    }
}
