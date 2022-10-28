using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MapGenerator : MonoBehaviour
{
    /// <summary>â°ïù</summary>
    [SerializeField] int _width = 1;
    /// <summary>ècïù</summary>
    [SerializeField] int _height = 1;
    RandomMapController _generateMap;

    void Start()
    {
        _generateMap = FindObjectOfType<RandomMapController>();
        _generateMap.GenerateMap(_width, _height);
    }
}
