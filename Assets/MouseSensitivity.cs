using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

// 日本語対応
public class MouseSensitivity : MonoBehaviour, IPauseResume
{
    public GameObject Player { private get; set; }
    private CinemachineVirtualCamera _virtualCamera = null;
    private CinemachinePOV _playerView = null;
    [SerializeField]
    private Slider _mouseSensitivitySlider = null;
    [SerializeField]
    private float _maxSensitivity = 200f;
    [SerializeField]
    private float _minSensitivity = 40f;

    //=====Pause Resume用変数=====
    private GameManager _gameManager = null;
    //private float _tempSeveValue = 0f;

    //private void Awake()
    //{
    //    _gameManager = FindObjectOfType<GameManager>();
    //}

    //private void OnEnable()
    //{
    //    _gameManager.OnPause += Pause;
    //    _gameManager.OnResume += Resume;
    //}

    //private void OnDisable()
    //{
    //    _gameManager.OnPause -= Pause;
    //    _gameManager.OnResume -= Resume;
    //}

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
        _virtualCamera = Player.GetComponentInChildren<CinemachineVirtualCamera>();
        _playerView = _virtualCamera?.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        if (_mouseSensitivitySlider == null) return;

        _playerView.m_HorizontalAxis.m_MaxSpeed = _mouseSensitivitySlider.value;
        _playerView.m_VerticalAxis.m_MaxSpeed = _mouseSensitivitySlider.value;
    }

    public void Pause()
    {
        
    }

    public void Resume()
    {
        
    }
}
