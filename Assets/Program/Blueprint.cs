using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class Blueprint : MonoBehaviour
{
    /// <summary>迷路を文字列にして表示する</summary>
    /// <param name="maze">迷路</param>
    /// <returns>文字列化した迷路</returns>
    public string ArrayToString(string[,] maze)
    {
        string str = "";

        for (int i = 0; i < maze.GetLength(1); i++)
        {
            for (int j = 0; j < maze.GetLength(0); j++)
            {
                str += maze[i, j];
            }
            if (i < maze.Length - 1) str += "\n";
        }
        Debug.Log(str);
        return str;
    }
}
