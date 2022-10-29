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
    SpriteRenderer _sprite = default;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _image.color = Color.black;
        _image.enabled = true;
        _image.DOFade(1f, _fadeTime);
    }

    public void SceneToLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
