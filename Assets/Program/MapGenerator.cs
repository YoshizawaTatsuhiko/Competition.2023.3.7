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
    private MazeCreaterExtend _maze = null;

    private void Start()
    {
        _maze = FindObjectOfType<MazeCreaterExtend>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _maze.GenerateMaze(_width, _height);
        }
    }
}
