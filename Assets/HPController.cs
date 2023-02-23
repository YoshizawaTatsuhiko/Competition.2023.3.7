using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 日本語対応
public class HPController : MonoBehaviour
{
    private float _currentHP = 1f;
    public float CurrentHP { get => _currentHP; set => _currentHP = Mathf.Clamp(value, 0f, _maxHP); }
    [SerializeField]
    private float _maxHP = 10f;

    private void Start()
    {
        CurrentHP = _maxHP;
    }

    private void Update()
    {
        if (CurrentHP == 0f) Destroy(gameObject);
    }
}
