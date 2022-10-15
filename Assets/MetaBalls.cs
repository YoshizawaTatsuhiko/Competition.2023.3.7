using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBalls : MonoBehaviour
{
    Material _mat = default;
    ParticleSystem _particleSystem = default;
    ParticleSystem.Particle[] _particles = default;
    List<Vector4> _particlesPos;
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

    public void TogglePlay(bool b)
    {
        if(b)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Pause();
        }
    }

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

    public void ToggleNoise(bool b)
    {
        ParticleSystem.NoiseModule n = _particleSystem.noise;
        n.enabled = b;
    }

    public void ChangeSpeed(float newSpeed)
    {
        ParticleSystem.MainModule main = _particleSystem.main;
        main.startSpeedMultiplier = newSpeed * _speed;
    }
}
