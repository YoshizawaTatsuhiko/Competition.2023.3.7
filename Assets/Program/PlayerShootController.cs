using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 日本語対応
public class PlayerShootController : ShootBase
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
    private float _shootRange = 1f;
    [SerializeField]
    private LayerMask _layerMask = 0;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(_crosshair.rectTransform.position);
        Vector3 hitPosition = _muzzle.transform.position + _muzzle.transform.forward * _shootRange;

        if (Physics.Raycast(ray, out RaycastHit hit, _shootRange, _layerMask))
        {
            _crosshair.color = _hitColor;
            hitPosition = hit.point;
        }
        else
        {
            _crosshair.color = _defaultColor;
        }

        if (Input.GetMouseButtonDown(1))
        {
            DrawRay(_muzzle.position, hitPosition);
        }
        else
        {
            DrawRay(_muzzle.position, _muzzle.position);
        }
    }
}
