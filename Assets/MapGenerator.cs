using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MapGenerator : MonoBehaviour
{
    /// <summary>‰¡•</summary>
    [SerializeField] int _width = 1;
    /// <summary>c•</summary>
    [SerializeField] int _height = 1;
    /// <summary>–À˜H‚ğŒ`ì‚éobject</summary>
    [SerializeField] GameObject[] _go = default;
    RandomMapController _generateMap;

    void Start()
    {
        _generateMap = FindObjectOfType<RandomMapController>();
        if (_generateMap.GenerateMap(_width, _height) == "W") { Instantiate(_go[0]); }
        if (_generateMap.GenerateMap(_width, _height) == "F") { Instantiate(_go[1]); }
        if (_generateMap.GenerateMap(_width, _height) == "S") { Instantiate(_go[2]); }
        if (_generateMap.GenerateMap(_width, _height) == "G") { Instantiate(_go[3]); }
    }
}
