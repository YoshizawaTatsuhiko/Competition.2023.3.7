using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class EnemyShootController : ShooterBase
{
    [SerializeField]
    private Transform _muzzle = null;
    [SerializeField]
    private float _shootRange = 1f;
    [SerializeField]
    private LayerMask _layerMask = 0;
    [SerializeField]
    private float _addDamage = 1f;
    [SerializeField]
    private float _lookTime = 1f;
    private float _timer = 0f;

    private void Start()
    {
        if (!_muzzle) _muzzle = transform;
    }

    public void EnemyShooter()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _shootRange, _layerMask);
        Debug.DrawRay(transform.position, transform.forward * _shootRange);

        if (!hit.collider) return;

        if (hit.collider.gameObject.TryGetComponent(out PlayerController player))
        {
            _timer += Time.deltaTime;

            if (_timer > _lookTime)
            {
                StartCoroutine(DrawLaser(_muzzle.position, hit.point));

                if (player.gameObject.TryGetComponent(out LifeController objectLife))
                {
                    objectLife.ChangeLife(_addDamage);
                }
                _timer = 0f;
            }
        }
        else
        {
            _timer = 0f;
        }
    }
}
