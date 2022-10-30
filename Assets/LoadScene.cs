using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class LoadScene : MonoBehaviour
{
    [SerializeField] float _fadeTime = 1f;
    [SerializeField] Image _image = default;

    void Start()
    {
        _image.color = Color.black;
        _image.enabled = true;
        _image.DOFade(0f, _fadeTime).OnComplete(() => _image.enabled = false);
    }

    public void SceneToLoad(string sceneName)
    {
        _image.enabled = true;
        _image.DOFade(1f, _fadeTime).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
