using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ql‘—¿
//https://soysoftware.sakura.ne.jp/archives/1559

/// <summary>—±q</summary>
class ParticleElements
{
    /// <summary>À•W</summary>
    public Vector3 position;
    /// <summary>‘¬“x</summary>
    public Vector3 velocity;
    /// <summary>—Í</summary>
    public Vector3 force;
    /// <summary>–§“x</summary>
    public float density;
    /// <summary>ˆ³—Í</summary>
    public float pressure;
}

class LiquidController : MonoBehaviour
{
    /// <summary>‰e‹¿”ÍˆÍ</summary>
    [SerializeField] float _areaOfInfluence = 0f;
    /// <summary>—±q‚Ì¿—Ê</summary>
    [SerializeField] float _particleMass = 1f;
    /// <summary>–§“x‚ÌŒvZ‚Åg‚¤</summary>
    [SerializeField] float _density = 1f;

    //[|||[||ˆ³—Í‚ÉŠÖ‚·‚éˆ—Œn[|||[||

    /// <summary>ˆ³—ÍŒW”</summary>
    [SerializeField] float _pressureCoefficient = 200f;
    /// <summary>ŠO—Í‚ª‚©‚©‚Á‚Ä‚È‚¢‚Æ‚«‚Ì–§“x</summary>
    [SerializeField] float _restDensity = 1000f;
    [SerializeField] float _pressure = 1f;

    void Start()
    {
        _density = 315 / 64 * Mathf.PI * Mathf.Pow(_areaOfInfluence, 9);  //–§“x‚ÌŒvZ
        _pressure = 45 / Mathf.PI * Mathf.Pow(_areaOfInfluence, 6);  //ˆ³—Í‚ÌŒvZ
    }

    //[|||||[–§“x‚ÉŠÖ‚·‚éˆ—Œn[||[|||

    /// <summary>—±q‚Ì–§“x‚ÌŒvZ</summary>
    /// <param name="particles">—±q‚Ì—v‘f</param>
    void CalcLiquid(ParticleElements[] particles)
    {
        float aoi2 = _areaOfInfluence * _areaOfInfluence;  //_areaOfInfluence ‚Ì2æ‚ğŒvZ‚µ‚Ä‚¨‚­

        for(int i = 0; i < particles.Length; i++)
        {
            var nowParticle = particles[i];
            float sumDensity = 0f;
            particles[i].pressure = _pressureCoefficient * (particles[i].density - _restDensity);
            //float sumPressure = 0f;


            for (int j = 0; j < particles.Length; j++)
            {
                if (i == j) continue;  //”»’è‚µ‚Ä‚¢‚é‚Ì‚ª©•ª©g‚¾‚Á‚½‚çAƒXƒLƒbƒv‚·‚é

                var nearParticle = particles[j];
                Vector3 particleDistence = nearParticle.position - nowParticle.position;
                float pd2 = Vector3.Dot(particleDistence, particleDistence);  //particle‚Ì“àÏ‚ğ‚Æ‚é

                if(pd2 < aoi2)
                {
                    sumDensity += Mathf.Pow(aoi2 - pd2, 3);
                }
            }
            nowParticle.density = sumDensity * _density;            
        }
    }
}
