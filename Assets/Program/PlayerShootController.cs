using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// 日本語対応
[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]

public class PlayerShootController : ShooterBase
{
    [SerializeField]
    private Transform _muzzle = null;
    [SerializeField]
    private Image _crosshair = null;
    [SerializeField]
    private Color _defaultColor = Color.white;
    [SerializeField]
    private Color _hitColor = Color.red;
    [SerializeField]
    private Color _hitGimmickColor = Color.yellow;
    [SerializeField]
    private float _shootRange = 1f;
    [SerializeField]
    private LayerMask _layerMask = 0;
    private RaycastHit _hit = new RaycastHit();
    [SerializeField]
    private UnityEvent _onShot = new();
    private float _addDamage = 1f;

    public void PlayerShooter()
    {
        string judge = "";
        Vector3 hitPosition = _muzzle.transform.position + Camera.main.transform.forward * _shootRange;
        Ray ray = Camera.main.ScreenPointToRay(_crosshair.rectTransform.position);
        Physics.Raycast(ray, out _hit, _shootRange, _layerMask);

        if (_hit.collider == null) return;

        if (_hit.collider.gameObject.TryGetComponent(out EnemyController Enemy))
        {
            judge = Enemy.gameObject.tag;
            _crosshair.color = _hitColor;
            hitPosition = _hit.point;
        }
        else if (_hit.collider.gameObject.TryGetComponent(out GimmickController gimmick))
        {
            _crosshair.color = _hitGimmickColor;
        }
        else
        {
            _crosshair.color = _defaultColor;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DrawLaser(_muzzle.position, hitPosition));
            _onShot.Invoke();

            if (_hit.collider?.tag == judge)
            {
                if (_hit.collider.gameObject.TryGetComponent(out LifeController objectLife))
                {
                    objectLife.ChangeLife(_addDamage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _shootRange);
    }
}
