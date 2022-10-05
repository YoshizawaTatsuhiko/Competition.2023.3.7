using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考資料
//https://soysoftware.sakura.ne.jp/archives/1559

class ParticleElements
{
    /// <summary>座標</summary>
    public Vector3 position;
    /// <summary>速度</summary>
    public Vector3 velocity;
    /// <summary>力</summary>
    public Vector3 force;
    /// <summary>密度</summary>
    public float density;
    /// <summary>圧力</summary>
    public float pressure;
}

public class LiquidController : MonoBehaviour
{
    /// <summary>影響範囲</summary>
    [SerializeField] float _areaOfInfluence = 0f;
    /// <summary>粒子の質量</summary>
    [SerializeField] float _particleMass = 1f;
    /// <summary>密度</summary>
    [SerializeField] float _density = 1f;
    void Start()
    {
        _density = 315 / 64 * Mathf.PI * Mathf.Pow(_areaOfInfluence, 9);  //密度の計算
    }

    void Update()
    {
        
    }
}
