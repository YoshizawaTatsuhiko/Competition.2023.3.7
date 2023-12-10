using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

// 日本語対応
public class MouseSensitivity : MonoBehaviour
{
    public PlayerController Player { get; set; }
    private CinemachineVirtualCamera _virtualCamera = null;
    private CinemachinePOV _playerView = null;
    [SerializeField]
    private Slider _mouseSensitivitySlider = null;
    [SerializeField]
    private float _maxSensitivity = 200f;
    [SerializeField]
    private float _minSensitivity = 40f;

    private void Start()
    {
        if (_mouseSensitivitySlider == null)
        {
            Debug.LogWarning("Slider isn't assigned");
        }
        else
        {
            _mouseSensitivitySlider.maxValue = _maxSensitivity;
            _mouseSensitivitySlider.minValue = _minSensitivity;
            _mouseSensitivitySlider.value = _maxSensitivity;
        }
        _virtualCamera = Player.transform.parent.GetComponentInChildren<CinemachineVirtualCamera>();
        _playerView = _virtualCamera?.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        if (_mouseSensitivitySlider == null) return;

        _playerView.m_HorizontalAxis.m_MaxSpeed = _mouseSensitivitySlider.value;
        _playerView.m_VerticalAxis.m_MaxSpeed = _mouseSensitivitySlider.value;
    }

    private float _saveSensitivity = 0.0f;

    public void ViewLock()
    {
        _saveSensitivity = _mouseSensitivitySlider.value;
        _playerView.m_HorizontalAxis.m_MaxSpeed = 0.0f;
        _playerView.m_VerticalAxis.m_MaxSpeed = 0.0f;
    }

    public void ViewUnlock()
    {
        _playerView.m_HorizontalAxis.m_MaxSpeed = _saveSensitivity;
        _playerView.m_VerticalAxis.m_MaxSpeed = _saveSensitivity;
    }
}
