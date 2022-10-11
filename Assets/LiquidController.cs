using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//参考資料
//https://soysoftware.sakura.ne.jp/archives/1559

/// <summary>粒子</summary>
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

class LiquidController : MonoBehaviour
{
    /// <summary>影響範囲</summary>
    [SerializeField] float _areaOfInfluence = 0f;
    /// <summary>粒子の質量</summary>
    [SerializeField] float _particleMass = 1f;
    /// <summary>密度の計算で使う</summary>
    [SerializeField] float _density = 1f;

    //ー−−−ー−−圧力に関する処理系ー−−−ー−−

    /// <summary>圧力係数</summary>
    [SerializeField] float _pressureCoefficient = 200f;
    /// <summary>外力がかかってないときの密度</summary>
    [SerializeField] float _restDensity = 1000f;
    [SerializeField] float _pressure = 1f;

    void Start()
    {
        _density = 315 / 64 * Mathf.PI * Mathf.Pow(_areaOfInfluence, 9);  //密度の計算
        _pressure = 45 / Mathf.PI * Mathf.Pow(_areaOfInfluence, 6);  //圧力の計算
    }

    //ー−−−−−ー密度に関する処理系ー−−ー−−−

    /// <summary>粒子の密度の計算</summary>
    /// <param name="particles">粒子の要素</param>
    void CalcLiquid(ParticleElements[] particles)
    {
        float aoi2 = _areaOfInfluence * _areaOfInfluence;  //_areaOfInfluence の2乗を計算しておく

        for(int i = 0; i < particles.Length; i++)
        {
            var nowParticle = particles[i];
            float sumDensity = 0f;
            particles[i].pressure = _pressureCoefficient * (particles[i].density - _restDensity);


            for (int j = 0; j < particles.Length; j++)
            {
                if (i == j) continue;  //判定しているのが自分自身だったら、スキップする

                var nearParticle = particles[j];
                Vector3 particleDistence = nearParticle.position - nowParticle.position;
                float pd2 = Vector3.Dot(particleDistence, particleDistence);  //particleの内積をとる

                if(pd2 < aoi2)
                {
                    sumDensity += Mathf.Pow(aoi2 - pd2, 3);
                }
            }
            nowParticle.density = sumDensity * _density;            
        }
    }
}
