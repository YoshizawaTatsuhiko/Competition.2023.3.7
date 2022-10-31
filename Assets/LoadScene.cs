using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class LoadScene : MonoBehaviour
{
    [Header("Fade in/out するまでの時間")]
    [Tooltip("Fade in/out するまでの時間")] [SerializeField] float _fadeTime = 1f;
    [Header("Fade in/out させるPanel")]
    [Tooltip("Fade in/out させるPanel")]    [SerializeField] Image _image = default;

    void Start()
    {
        //Panelを見えるようにしておく
        _image?.CrossFadeAlpha(1, 0, false);
        //Fade in 完了後、Panelを消す
        _image.enabled = true;
        _image.DOFade(0f, _fadeTime).OnComplete(() => _image.enabled = false);
    }

    /// <summary>シーンを遷移させる</summary>
    /// <param name="sceneName">シーンの名前</param>
    public void SceneToLoad(string sceneName)
    {
        //Panelを見えなくしておく
        _image.CrossFadeAlpha(1, 0, false);
        //Fade out 完了後、シーンを遷移する
        _image.enabled = true;
        _image.DOFade(1f, _fadeTime).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
