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

    private void Start()
    {
        _width = _size;
        _height = _size;
        _maze = GetComponent<MazeCreaterExtend>();
        string[] mazeInfo = _maze.GenerateMaze(_width, _height).Split("\n");
        _bluePrint = new string[mazeInfo[0].Length, mazeInfo.Length - 1];
        To2DArray(mazeInfo, _bluePrint);

        GameObject wallParent = new GameObject("Wall Parent");
        GameObject flooeParent = new GameObject("Floor Parent");

        for (int i = 0; i < _bluePrint.GetLength(0); i++)
        {
            for (int j = 0; j < _bluePrint.GetLength(1); j++)
            {
                if (_bluePrint[i, j] == "W") Instantiate(_go[0], 
                    new Vector3(i - _size / 2, 0, j - _size / 2), Quaternion.identity, wallParent.transform);
                if (_bluePrint[i, j] == "F") Instantiate(_go[1], 
                    new Vector3(i - _size / 2, -0.5f, j - _size / 2), Quaternion.identity, flooeParent.transform);
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
