using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class LoadScene : MonoBehaviour
{
    [SerializeField, Header("Fade in/out するまでの時間")]
    private float _fadeTime = 1f;
    [SerializeField, Header("Fade in/out させるPanel")]
    private Image _image = null;

    void Start()
    {
        if (_image != null)
        {
            //Panelを見えるようにしておく
            _image.CrossFadeAlpha(1, 0, false);
            //Fade in 完了後、Panelを消す
            _image.enabled = true;
            _image.DOFade(0f, _fadeTime).OnComplete(() => _image.enabled = false);
        }
    }

    /// <summary>シーンを遷移させる</summary>
    /// <param name="sceneName">シーンの名前</param>
    public void SceneToLoad(string sceneName)
    {
        if (_image != null)
        {
            //Panelを見えなくしておく
            _image.CrossFadeAlpha(1, 0, false);
            //Fade out 完了後、シーンを遷移する
            _image.enabled = true;
            _image.DOFade(1f, _fadeTime).OnComplete(() => SceneManager.LoadScene(sceneName));
        }
    }
}
