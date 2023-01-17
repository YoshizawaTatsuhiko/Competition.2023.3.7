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
            string mazeInfo = _maze.GenerateMaze(_width, _height);
            //if (mazeInfo == "W") Instantiate(_go[0]);
            //if (mazeInfo == "F") Instantiate(_go[1]);
            _frag = false;
        }
        else
        {
            _timer += Time.fixedDeltaTime;

            if (_timer > 3f)
            {
                _frag = true;
                _timer = 0f;
            }
        }
    }
}
