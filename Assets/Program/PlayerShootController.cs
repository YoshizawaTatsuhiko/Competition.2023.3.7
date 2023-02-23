using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 日本語対応
[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]

public class PlayerShootController : MonoBehaviour
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
    private RaycastHit _hit = new RaycastHit();
    private LineRenderer _line = null;
    [SerializeField]
    private float _addDamage = 1f;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3 hitPosition = _muzzle.transform.position + _muzzle.transform.forward * _shootRange;

        if (Physics.Raycast(Camera.main.transform.position,
            Camera.main.transform.forward, out _hit, _shootRange, _layerMask))
        {
            _crosshair.color = _hitColor;
            hitPosition = _hit.point;
        }
        else
        {
            _crosshair.color = _defaultColor;
        }

        if (Input.GetMouseButtonDown(0))
        {
            DrawLaser(hitPosition);

            if (_hit.collider?.tag is "Enemy")
            {
                if (_hit.collider.gameObject.TryGetComponent(out HPController hp))
                {
                    hp.CurrentHP -= _addDamage;
                    Debug.Log($"{_hit.collider.gameObject.name}'s Health = {hp.CurrentHP}");
                }
            }
        }
        else
        {
            DrawLaser(_muzzle.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _shootRange);
    }

    /// <summary>銃弾の軌跡を表す光線</summary>
    /// <param name="destination">光線の終点</param>
    private void DrawLaser(Vector3 destination)
    {
        Vector3[] positions = { _muzzle.position, destination };
        _line.positionCount = positions.Length;
        _line.SetPositions(positions);
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
