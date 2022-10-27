using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MapGenerator : MonoBehaviour
{
    [SerializeField] int _width = 1;
    [SerializeField] int _height = 1;
    RandomMapController _generateMap;

    void Start()
    {
        _generateMap = FindObjectOfType<RandomMapController>();
        _generateMap.GenerateMap(_width, _height);
    }
}
