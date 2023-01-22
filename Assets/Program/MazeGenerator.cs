using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeCreaterExtend))]

class MazeGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("MAX SIZE = 32")]
    private int _size = 5;
    private int _width = 0;
    private int _height = 0;
    [SerializeField, Tooltip("–À˜H‚ğŒ`¬‚·‚éobject")]
    private GameObject[] _go = null;
    /// <summary>¶¬‚µ‚½–À˜H</summary>
    private MazeCreaterExtend _maze = null;
    private string[,] _bluePrint;

    [SerializeField]
    private float _clickInterval = 2f;
    private float _timer = 0f;

    private void Start()
    {
        _width = _size;
        _height = _size;
        _maze = GetComponent<MazeCreaterExtend>();
        string[] mazeInfo = _maze.GenerateMaze(_width, _height).Split("\n");
        _bluePrint = new string[mazeInfo[0].Length, mazeInfo.Length - 1];
        To2DArray(mazeInfo, _bluePrint);

        for (int i = 0; i < _bluePrint.GetLength(0); i++)
        {
            for (int j = 0; j < _bluePrint.GetLength(1); j++)
            {
                if (_bluePrint[i, j] == "W") Instantiate(_go[0], new Vector3(i, 0, j), Quaternion.identity);
                if (_bluePrint[i, j] == "F") Instantiate(_go[1], new Vector3(i, 0, j), Quaternion.identity);
            }
        }
    }

    private bool _frag = true;

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1) && _frag)
        {
            //string[] mazeInfo = _maze.GenerateMaze(_width, _height).Split("\n");
            //_bluePrint = new string[mazeInfo[0].Length, mazeInfo.Length];
            //To2DArray(mazeInfo, _bluePrint);

            //for (int i = 0; i < _height; i++)
            //{
            //    for(int j = 0; j < _width; j++)
            //    {
            //        if (_bluePrint[i, j] == "W") Instantiate(_go[0], new Vector3(i, 0, j), Quaternion.identity);
            //        if (_bluePrint[i, j] == "F") Instantiate(_go[1], new Vector3(i, 0, j), Quaternion.identity);
            //    }
            //}
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

    /// <summary>ˆêŸŒ³”z—ñ‚ğ“ñŸŒ³”z—ñ‚É•ÏŠ·‚·‚é(stringŒ^ŒÀ’è)</summary>
    /// <param name="array">stringŒ^‚Ì”z—ñ</param>
    /// <param name="twoDimensionalArray">stringŒ^‚Ì“ñŸŒ³”z—ñ</param>
    private string[,] To2DArray(string[] array, string[,] twoDimensionalArray)
    {
        for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
            {
                twoDimensionalArray[i, j] = array[i][j].ToString();
            }
        }
        return twoDimensionalArray;
    }
}
