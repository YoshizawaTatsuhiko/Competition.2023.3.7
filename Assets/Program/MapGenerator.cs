using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int _width = 1;
    [SerializeField]
    private int _height = 1;
    [SerializeField, Tooltip("ñ¿òHÇå`ê¨Ç∑ÇÈobject")]
    private GameObject[] _go = null;
    /// <summary></summary>
    MazeMakeController _generateMap;

    private void Start()
    {
        _generateMap = FindObjectOfType<MazeMakeController>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
        }
    }
}
