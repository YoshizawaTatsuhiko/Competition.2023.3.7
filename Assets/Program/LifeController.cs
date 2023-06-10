using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 日本語対応
public class LifeController : MonoBehaviour
{
    private float _currentLife = 1f;
    public float CurrentLife => _currentLife;
    [SerializeField]
    private float _maxLife = 10f;
    [SerializeField]
    private UnityEvent _onReduceLife = new();
    [SerializeField]
    private UnityEvent _onAddLife = new();

    private void Start()
    {
        _currentLife = _maxLife;
    }

    private void Update()
    {
        if (_currentLife == 0f) Destroy(gameObject);
    }

    /// <summary>Lifeの値を変化させる</summary>
    /// <param name="changeValue">変化させる値</param>
    /// <param name="isReduceLife">true -> Life を changeValue 減らす | false -> Life を changeValue 増やす</param>
    public void ChangeLife(float changeValue, bool isReduceLife = true)
    {
        if (isReduceLife)
        {
            _currentLife -= changeValue;
            _onReduceLife.Invoke();
        }
        else
        {
            _currentLife += changeValue;
            _onAddLife.Invoke();
        }
        Debug.Log($"{gameObject.name}'s Health = {_currentLife}");
    }
}
