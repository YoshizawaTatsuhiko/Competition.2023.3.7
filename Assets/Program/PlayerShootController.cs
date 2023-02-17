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
    private LineRenderer _line = null;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3 hitPosition = _muzzle.transform.position + _muzzle.transform.forward * _shootRange;

        if (Physics.Raycast(Camera.main.transform.position,
            Camera.main.transform.forward, out RaycastHit hit, _shootRange, _layerMask))
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
            DrawTrajectory(hitPosition);
        }
        else
        {
            DrawTrajectory(_muzzle.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _shootRange);
    }

    /// <summary>銃弾の軌跡を表す光線</summary>
    /// <param name="destination">光線の終点</param>
    private void DrawTrajectory(Vector3 destination)
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
