using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : MonoBehaviour
{
    /// <summar>対象のマテリアル</summary>
    Material _mat = default;
    /// <summary></summary>
    ParticleSystem _particleSystem = default;
    /// <summary></summary>
    ParticleSystem.Particle[] _particles = default;
    /// <summary></summary>
    List<Vector4> _particlesPos;
    /// <summary>パーティクルの速度</summary>
    float _speed = 1f;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        _particlesPos = new List<Vector4>(10);
        _mat = GetComponent<ParticleSystemRenderer>().sharedMaterial;
        _speed = _particleSystem.main.startSpeedMultiplier;
    }

    void Update()
    {
        _particlesPos.Clear();
        int arriveParticle = _particleSystem.GetParticles(_particles);

        for(int i = 0; i < arriveParticle; i++)
        {
            _particlesPos.Add(_particles[i].position);
        }
        _mat.GetVectorArray("PrticlePos", _particlesPos);
    }

    /// <summary>パーティクルの動作(ON/OFF)を切り替える</summary>
    /// <returns></returns>
    public bool TogglePlay()
    {
        if (_particleSystem.isPlaying)
        {
            _particleSystem.Pause();
        }
        else
        {
            _particleSystem.Play();
        }
        return _particleSystem.isPlaying;
    }

    /// <summary>パーティクルの動作(ON/OFF)を切り替える</summary>
    /// <param name="exchange">ON/OFFを切り替える</param>
    public void TogglePlay(bool exchange)
    {
        if(exchange)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Pause();
        }
    }

    /// <summary>パーティクルに乱気流(ばらつき)を発生させる</summary>
    /// <returns></returns>
    public bool ToggleNoise()
    {
        ParticleSystem.NoiseModule n = _particleSystem.noise;

        if(n.enabled)
        {
            n.enabled = false;
        }
        else
        {
            n.enabled = true;
        }
        return n.enabled;
    }

    /// <summary></summary>
    /// <param name="exchange"></param>
    public void ToggleNoise(bool exchange)
    {
        ParticleSystem.NoiseModule n = _particleSystem.noise;
        n.enabled = exchange;
    }

    /// <summary>パーティクルの速度を変化させる</summary>
    /// <param name="newSpeed">速度更新の値</param>
    public void ChangeSpeed(float newSpeed)
    {
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startSpeedMultiplier = newSpeed * _speed;
    }
}
