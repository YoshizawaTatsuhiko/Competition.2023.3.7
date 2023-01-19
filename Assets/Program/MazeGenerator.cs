using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeCreaterExtend))]

class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private int _width = 1;
    [SerializeField]
    private int _height = 1;
    [SerializeField, Tooltip("ñ¿òHÇå`ê¨Ç∑ÇÈobject")]
    private GameObject[] _go = null;
    /// <summary>ê∂ê¨ÇµÇΩñ¿òH</summary>
    private MazeCreaterExtend _maze = null;

    [SerializeField]
    private float _clickInterval = 2f;
    private float _timer = 0f;

    private void Start()
    {
        _maze = GetComponent<MazeCreaterExtend>();
    }

    private bool _frag = true;

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && _frag)
        {
            string[] mazeInfo = _maze.GenerateMaze(_width, _height).Split("\n");

            for (int i = 0; i < _height; i++)
            {
                for(int j = 0; j < _width; j++)
                {
                    if (mazeInfo[i][j] == 'W') Instantiate(_go[0]);
                    if (mazeInfo[i][j] == 'F') Instantiate(_go[1]);
                }
            }
            _frag = false;
        }
        else
        {
            _timer += Time.fixedDeltaTime;

            if (_timer > _clickInterval)
            {
                _frag = true;
                _timer = 0f;
            }
        }
    }
}
