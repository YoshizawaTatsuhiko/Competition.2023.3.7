using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

// 日本語対応
[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]

public class PlayerShootController : MonoBehaviour
{
    private Camera _mainCamera = null;
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
    private LineRenderer _line = null;

    private void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_mainCamera.transform.position);
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

        if (Input.GetMouseButton/*Down*/(0))
        {
            //DrawRay(_muzzle.position, hitPosition);
            DrawLaser(hitPosition);
            //StartCoroutine(WaitForSeconds());
        }
        else
        {
            //DrawRay(_muzzle.position, _muzzle.position);
            DrawLaser(_muzzle.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_mainCamera.transform.position, _mainCamera.transform.forward * _shootRange);
    }

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
