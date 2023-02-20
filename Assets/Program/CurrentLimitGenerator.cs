using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLimitGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _object = null;
    [SerializeField, Tooltip("最大でいくつ同時に存在させるか")]
    private int _simultaneousCount = 1;
    [SerializeField]
    private float _interval = 1f;
    private bool _isInstantiate = true;

    private void Update()
    {
        if(_isInstantiate) StartCoroutine(Generate());
    }

    /// <summary>同時最大生成数を制限したGenerator</summary>
    public IEnumerator Generate()
    {
        _isInstantiate = false;

        int n = Random.Range(0, _object.Length);
        if (transform.childCount < _simultaneousCount)
        {
            Instantiate(_object[n], transform);
        }
        yield return new WaitForSeconds(_interval);

        _isInstantiate = true;
    }
}
